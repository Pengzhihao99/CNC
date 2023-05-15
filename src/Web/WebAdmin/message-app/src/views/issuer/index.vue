<template>
  <div>
    <div class="search-box">
      <a-form layout="inline" :model="formStateForSearch">
        <a-form-item>
          <a-input v-model:value="formStateForSearch.name" allowClear="true" placeholder="发起人" />
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
      :columns="columns"
      :row-key="record => record.id"
      :data-source="dataSource"
      :pagination="false"
      :loading="loading"
    >
      <template #enabled="{ text }">
        <span v-if="text">
          <CheckOutlined :style="{ fontSize: '20px', color: '#08c' }" />
        </span>
        <span v-else>
          <CloseOutlined :style="{ fontSize: '20px', color: 'red' }" />
        </span>
      </template>
      <template #updateOn="{ text }">
        {{ moment(text).format('YYYY-MM-DD HH:mm:ss') }}
      </template>
      <template #action="{ record }">
        <span>
          <a-button type="link" @click="handleView(record)">{{ OperationEnum.VIEW }}</a-button>
          <a-divider type="vertical" />
          <a-button type="link" @click="handleEdit(record)">{{ OperationEnum.UPDATE }}</a-button>
        </span>
      </template>
    </a-table>
    <Paging :page-number="pageNumber" :page-size="pageSize" :total="total" @change="onChange" />

    <a-modal
      v-model:visible="visible"
      width="1000px"
      height="1000px"
      keyboard="true"
      :title="modelTitle"
      @ok="handleOk"
      @cancel="resetForm"
    >
      <div style="height: 250px; margin-top: 20px">
        <a-form
          ref="formRef"
          :model="senderInfo"
          :rules="rules"
          :label-col="{ span: 4 }"
          :wrapper-col="{ span: 14 }"
        >
          <a-form-item has-feedback label="发送者" ref="senderName" name="senderName">
            <a-input v-model:value="senderInfo.name" :disabled="modelTitle !== OperationEnum.ADD" />
          </a-form-item>
          <a-form-item label="状态">
            <a-switch v-model:checked="senderInfo.enabled" :disabled="isEditing" />
          </a-form-item>
          <a-form-item label="备注" ref="remark" name="remark">
            <a-textarea v-model:value="senderInfo.remark" type="textarea" :disabled="isEditing" />
          </a-form-item>
        </a-form>
        <!-- <a-form-item :wrapper-col="{ span: 14, offset: 4 }">
          <a-button type="primary" @click="onSubmit">确认</a-button>
          <a-button style="margin-left: 10px" @click="resetForm">取消</a-button>
        </a-form-item> -->
      </div>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue'
  import { Paging } from '/@/components/Paging'
  import moment from 'moment'
  import { reactive, ref, UnwrapRef } from 'vue'
  import { getIssuerByPage, addIssuer, updateIssuer } from '/@/api/issuer'
  import { ValidateErrorEntity } from 'ant-design-vue/es/form/interface'
  import { AddIssuer, IssuerVM, FormStateForSearch } from '/@/api/issuer/model'
  import { message } from 'ant-design-vue'
  import { OperationEnum } from '/@/enums/appEnum'

  const columns = [
    {
      title: '发送者',
      dataIndex: 'name',
      sorter: true,
      width: '20%',
      slots: { customRender: 'name' },
    },
    {
      title: 'Token',
      dataIndex: 'token',
      width: '20%',
    },
    {
      title: '状态',
      dataIndex: 'enabled',
      slots: { customRender: 'enabled' },
    },
    {
      title: '更新时间',
      dataIndex: 'updateOn',
      slots: { customRender: 'updateOn' },
    },
    {
      title: '备注',
      dataIndex: 'remark',
    },
    {
      title: '操作',
      dataIndex: 'action',
      //使用插槽
      slots: { customRender: 'action' },
    },
  ]

  //显示 查询
  const dataSource = ref<IssuerVM[]>([])
  const pageSize = ref<number>(10)
  const pageNumber = ref<number>(1)
  const total = ref<number>(1)
  const loading = ref<Boolean>(true)
  const formStateForSearch: UnwrapRef<FormStateForSearch> = reactive({
    name: '',
  })
  const fetchData = () => {
    loading.value = true
    getIssuerByPage({
      pageSize: pageSize.value,
      pageNumber: pageNumber.value,
      paging: true,
      name: formStateForSearch.name,
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

  //表单  增删查改
  const formRef = ref()
  const visible = ref<boolean>(false)
  const isEditing = ref<boolean>(false)
  const modelTitle = ref<String>('')
  const rules = {
    name: [
      { required: true, message: '请输入发起人名称！', trigger: 'blur' },
      { min: 1, max: 20, message: '字符长度为1-20！', trigger: 'blur' },
    ],
    //remark: [{ min: 1, max: 100, message: '允许输入最大长度为100！', trigger: 'blur' }],
  }

  //确认
  const handleOk = () => {
    formRef.value
      .validate()
      .then(async () => {
        if (modelTitle.value == OperationEnum.VIEW) {
          visible.value = false
          return
        }
        //创建
        if (!senderInfo.id) {
          await addIssuer({
            name: senderInfo.name,
            enabled: senderInfo.enabled,
            remark: senderInfo.remark,
          }).then(() => {
            visible.value = false
            message.success('数据添加成功！')
          })
        }
        //更新
        else {
          await updateIssuer({
            id: senderInfo.id,
            enabled: senderInfo.enabled,
            remark: senderInfo.remark,
          }).then(() => {
            visible.value = false
            message.success('数据更新成功！')
          })
        }
        handleSearch()
      })
      .catch((error: ValidateErrorEntity<AddIssuer>) => {
        console.log('error', error)
      })
  }

  //取消
  const resetForm = () => {
    formRef.value.resetFields()
  }

  const senderInfo: UnwrapRef<AddIssuer> = reactive({
    id: '',
    name: '',
    enabled: false,
    remark: '',
  })

  const handleAdd = () => {
    isEditing.value = false
    modelTitle.value = OperationEnum.ADD
    visible.value = true
    senderInfo.id = ''
    senderInfo.name = ''
    senderInfo.enabled = true
    senderInfo.remark = ''
  }

  const handleEdit = (model: IssuerVM) => {
    isEditing.value = false
    modelTitle.value = OperationEnum.UPDATE
    visible.value = true

    senderInfo.name = model.name
    senderInfo.enabled = model.enabled
    senderInfo.remark = model.remark
    senderInfo.id = model.id
  }

  const handleView = (model: IssuerVM) => {
    isEditing.value = true
    modelTitle.value = OperationEnum.VIEW
    visible.value = true
    //const { senderName, status, remark } = JSON.parse(JSON.stringify(model))
    //senderInfo = reactive({ senderName, status, remark })
    senderInfo.name = model.name
    senderInfo.enabled = model.enabled
    senderInfo.remark = model.remark
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
