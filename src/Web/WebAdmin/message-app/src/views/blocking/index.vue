<template>
  <div>
    <div class="search-box">
      <a-form layout="inline" :model="formStateFroSearch">
        <a-form-item>
          <a-input v-model:value="formStateFroSearch.templateName" placeholder="模板" />
        </a-form-item>
        <a-form-item>
          <a-button type="primary" @click.prevent="handleSearch">查询</a-button>
        </a-form-item>
      </a-form>
    </div>

    <div class="add-box">
      <a-button type="primary" @click="handleAdd">添加</a-button>
    </div>

    <a-table
      :dataSource="dataSource"
      :columns="columns"
      rowKey="id"
      :pagination="false"
      :loading="loading"
    >
      <template #blacklists="{ text: blacklists }">
        <span>
          {{ blacklists != null && blacklists.length > 0 ? blacklists.join(',') : '' }}
        </span>
      </template>

      <template #action="{ record }">
        <span>
          <a-button type="link" @click="handleUpdate(record)">{{ OperationEnum.UPDATE }}</a-button>
        </span>
      </template>
    </a-table>
    <Paging :page-number="pageNumber" :page-size="pageSize" :total="total" @change="onChange" />

    <a-modal v-model:visible="modelVisible" width="1000px" :title="modelTitle" @ok="handleOk">
      <br />
      <a-form
        :model="formStateForOperation"
        :label-col="{ span: 6 }"
        :wrapper-col="{ span: 12 }"
        :rules="rules"
        ref="formForOperationRef"
      >
        <a-form-item label="模板" required name="templateName" has-feedback>
          <a-input v-model:value="formStateForOperation.templateName" />
        </a-form-item>
        <a-form-item label="黑名单" required name="blacklistsStr" has-feedback>
          <a-textarea v-model:value="formStateForOperation.blacklistsStr" />
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
  import type { Column } from '/#/antd/table'
  import { ref, UnwrapRef, reactive } from 'vue'
  import type { BlockingVM } from '/@/api/blocking/model'
  import { getBlockingByPage, createBlocking, updateBlocking } from '/@/api/blocking'
  import { Paging } from '/@/components/Paging'
  import { OperationEnum } from '/@/enums/appEnum'
  import { ValidateErrorEntity } from 'ant-design-vue/es/form/interface'
  import { message } from 'ant-design-vue'
  const columns: Column[] = [
    {
      title: 'Id',
      dataIndex: 'id',
      key: 'id',
    },
    {
      title: '模板',
      dataIndex: 'templateName',
      key: 'templateName',
    },
    {
      title: '创建时间',
      dataIndex: 'createOn',
      key: 'createOn',
    },
    {
      title: '更新时间',
      dataIndex: 'updateOn',
      key: 'updateOn',
    },
    {
      title: '黑名单',
      dataIndex: 'blacklists',
      key: 'blacklists',
      slots: { customRender: 'blacklists' },
    },
    {
      title: '操作',
      key: 'action',
      slots: { customRender: 'action' },
    },
  ]

  //table
  interface FormStateFroSearch {
    templateName: string
  }
  const formStateFroSearch: UnwrapRef<FormStateFroSearch> = reactive({
    templateName: '',
  })

  const loading = ref<Boolean>(true)
  const dataSource = ref<BlockingVM[]>([])
  const pageSize = ref<number>(10)
  const pageNumber = ref<number>(1)
  const total = ref<number>(1)
  const fetchData = () => {
    loading.value = true
    getBlockingByPage({
      pageSize: pageSize.value,
      pageNumber: pageNumber.value,
      paging: true,
      templateName: formStateFroSearch.templateName,
    }).then(rep => {
      dataSource.value = rep.data
      total.value = rep.totalRecords
      pageNumber.value = rep.pageNumber
      loading.value = false
    })
  }
  const onChange = (currentPageNumber: number, currentPageSize: number) => {
    pageNumber.value = currentPageNumber
    pageSize.value = currentPageSize
    fetchData()
  }

  fetchData()

  //search
  const handleSearch = () => {
    pageNumber.value = 1
    fetchData()
  }

  //add or update
  const rules = {
    templateName: [{ required: true, message: '*', trigger: 'blur' }],
    blacklistsStr: [{ required: true, message: '*', trigger: 'blur' }],
  }

  const modelVisible = ref<boolean>(false)
  const modelTitle = ref<String>('')
  const formForOperationRef = ref()

  interface FormStateForOperation {
    templateName: string
    blacklistsStr: string
    id: string
  }
  const formStateForOperation: UnwrapRef<FormStateForOperation> = reactive({
    templateName: '',
    blacklistsStr: '',
    id: '',
  })

  const handleAdd = () => {
    modelTitle.value = OperationEnum.ADD
    modelVisible.value = true

    formStateForOperation.templateName = ''
    formStateForOperation.blacklistsStr = ''
    formStateForOperation.id = ''
  }

  const handleUpdate = (model: BlockingVM) => {
    modelTitle.value = OperationEnum.UPDATE
    modelVisible.value = true

    formStateForOperation.templateName = model.templateName
    formStateForOperation.blacklistsStr =
      model.blacklists != null && model.blacklists.length > 0 ? model.blacklists.join(',') : ''
    formStateForOperation.id = model.id
  }

  const handleOk = () => {
    formForOperationRef.value
      .validate()
      .then(async () => {
        //console.log('success', formStateForOperation)
        //创建
        if (!formStateForOperation.id) {
          await createBlocking({
            templateName: formStateForOperation.templateName,
            blacklists: formStateForOperation.blacklistsStr.split(','),
          })
          modelVisible.value = false
          message.success('add success')
        } else {
          //更新
          await updateBlocking({
            templateName: formStateForOperation.templateName,
            blacklists: formStateForOperation.blacklistsStr.split(','),
            id: formStateForOperation.id,
          })
          modelVisible.value = false
          message.success('update success')
        }
        handleSearch()
      })
      .catch((error: ValidateErrorEntity<FormStateForOperation>) => {
        console.log('error', error)
      })
  }
</script>

<style lang="less" scoped>
  .add-box {
    margin-bottom: 10px;
    text-align: right;
  }
  .search-box {
  }
</style>
