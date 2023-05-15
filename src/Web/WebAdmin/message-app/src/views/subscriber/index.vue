<template>
  <div>
    <div class="search-box">
      <a-form layout="inline" :model="formStateForSearch">
        <a-form-item>
          <a-input
            v-model:value="formStateForSearch.group"
            allowClear="true"
            placeholder="订阅者代码"
          />
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
      <div style="margin-top: 20px">
        <a-form
          ref="formRef"
          :model="subscriberInfo"
          :rules="rules"
          :label-col="{ span: 4 }"
          :wrapper-col="{ span: 14 }"
        >
          <a-form-item has-feedback label="订阅者代码" ref="name" name="name">
            <a-input
              v-model:value="subscriberInfo.name"
              :disabled="modelTitle !== OperationEnum.ADD"
            />
          </a-form-item>
          <a-form-item has-feedback label="从属代码" ref="group" name="group">
            <a-input
              v-model:value="subscriberInfo.group"
              :disabled="modelTitle !== OperationEnum.ADD"
            />
          </a-form-item>
          <a-form-item has-feedback label="订阅者类型" name="subscriberType">
            <a-select
              v-model:value="subscriberInfo.subscriberType"
              placeholder="Please select"
              :options="subscriberTypeOptions"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item has-feedback label="手机号码" ref="phone" name="phone">
            <a-input
              v-model:value="subscriberInfo.phone"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item has-feedback label="邮件地址" ref="email" name="email">
            <a-input
              v-model:value="subscriberInfo.email"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item
            has-feedback
            label="企业微信账号"
            ref="enterpriseWeChat"
            name="enterpriseWeChat"
          >
            <a-input
              v-model:value="subscriberInfo.enterpriseWeChat"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item label="是否启用">
            <a-switch
              v-model:checked="subscriberInfo.enabled"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
        </a-form>
      </div>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue'
  import { Paging } from '/@/components/Paging'
  import moment from 'moment'
  import { reactive, ref, UnwrapRef } from 'vue'
  import { addSubscriber, getSubscribersByPage, updateSubscriber } from '/@/api/subscriber'
  import { AddSubscriber, SubscriberVM, FormStateForSearch } from '/@/api/subscriber/model'

  import { OperationEnum } from '/@/enums/appEnum'
  import { SubscriberTypeEnum } from '/@/enums/subscriberTypeEnum'
  import { message } from 'ant-design-vue'
  import { SelectTypes } from 'ant-design-vue/lib/select'
  import { ValidateErrorEntity } from 'ant-design-vue/lib/form/interface'
  import { CreateTemplateCommand } from '/@/api/template/model'

  const columns = [
    {
      title: '从属代码',
      dataIndex: 'group',
      sorter: true,
      width: '20%',
      slots: { customRender: 'group' },
    },
    {
      title: '订阅者代码',
      dataIndex: 'name',
      width: '20%',
    },
    {
      title: '订阅者类型',
      dataIndex: 'subscriberType.name',
    },
    {
      title: '手机号码',
      dataIndex: 'phone',
      slots: { customRender: 'phone' },
    },
    {
      title: '邮件地址',
      dataIndex: 'email',
      slots: { customRender: 'email' },
    },
    {
      title: '企业微信账号',
      dataIndex: 'enterpriseWeChat',
      slots: { customRender: 'enterpriseWeChat' },
    },
    {
      title: '是否启用',
      dataIndex: 'enabled',
      slots: { customRender: 'enabled' },
    },
    {
      title: '操作',
      dataIndex: 'action',
      //使用插槽
      slots: { customRender: 'action' },
    },
  ]

  //显示 查询
  const dataSource = ref<SubscriberVM[]>([])
  const pageSize = ref<number>(10)
  const pageNumber = ref<number>(1)
  const total = ref<number>(1)
  const loading = ref<Boolean>(true)
  const formStateForSearch: UnwrapRef<FormStateForSearch> = reactive({
    group: '',
  })

  const fetchData = () => {
    loading.value = true
    getSubscribersByPage({
      pageSize: pageSize.value,
      pageNumber: pageNumber.value,
      paging: true,
      group: formStateForSearch.group,
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
      { required: true, message: '请输入订阅者代码！', trigger: 'blur' },
      { min: 1, max: 20, message: '字符长度为1-20！', trigger: 'blur' },
    ],
    group: [
      { required: true, message: '请输入从属代码！', trigger: 'blur' },
      { min: 1, max: 20, message: '字符长度为1-20！', trigger: 'blur' },
    ],
  }

  //初始化表单操作对象
  const subscriberTypeOptions = ref<SelectTypes['options']>([
    { value: SubscriberTypeEnum.Client, label: '客户' },
    { value: SubscriberTypeEnum.Employee, label: '内部员工' },
  ])

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
        if (!subscriberInfo.id) {
          await addSubscriber({
            name: subscriberInfo.name,
            email: subscriberInfo.email,
            phone: subscriberInfo.phone,
            enterpriseWeChat: subscriberInfo.enterpriseWeChat,
            subscriberType: subscriberInfo.subscriberType,
            group: subscriberInfo.group,
            enabled: subscriberInfo.enabled,
          }).then(() => {
            visible.value = false
            message.success('数据添加成功!')
          })
        } else {
          await updateSubscriber({
            id: subscriberInfo.id,
            email: subscriberInfo.email,
            phone: subscriberInfo.phone,
            enterpriseWeChat: subscriberInfo.enterpriseWeChat,
            subscriberType: subscriberInfo.subscriberType,
            enabled: subscriberInfo.enabled,
          }).then(() => {
            visible.value = false
            message.success('数据更新成功!')
          })
        }
        handleSearch()
        resetForm()
      })
      .catch((error: ValidateErrorEntity<CreateTemplateCommand>) => {
        console.log('error', error)
      })
  }

  //取消
  const resetForm = () => {
    formRef.value.resetFields()
  }

  const subscriberInfo: UnwrapRef<AddSubscriber> = reactive({
    id: '',
    name: '',
    email: '',
    phone: '',
    enterpriseWeChat: '',
    subscriberType: '',
    group: '',
    enabled: true,
  })

  const handleAdd = () => {
    isEditing.value = false
    modelTitle.value = OperationEnum.ADD
    visible.value = true

    subscriberInfo.id = ''
    subscriberInfo.name = ''
    subscriberInfo.email = ''
    subscriberInfo.phone = ''
    subscriberInfo.enterpriseWeChat = ''
    subscriberInfo.subscriberType = SubscriberTypeEnum.Client
    subscriberInfo.group = ''
    subscriberInfo.enabled = true
  }

  const handleEdit = (model: SubscriberVM) => {
    isEditing.value = false
    modelTitle.value = OperationEnum.UPDATE
    visible.value = true

    subscriberInfo.id = model.id
    subscriberInfo.email = model.email
    subscriberInfo.phone = model.phone
    subscriberInfo.enterpriseWeChat = model.enterpriseWeChat
    subscriberInfo.subscriberType = model.subscriberType.id
    subscriberInfo.name = model.name
    subscriberInfo.group = model.group
    subscriberInfo.enabled = model.enabled
  }

  const handleView = (model: SubscriberVM) => {
    isEditing.value = true
    modelTitle.value = OperationEnum.VIEW
    visible.value = true

    subscriberInfo.id = model.id
    subscriberInfo.name = model.name
    subscriberInfo.email = model.email
    subscriberInfo.phone = model.phone
    subscriberInfo.enterpriseWeChat = model.enterpriseWeChat
    subscriberInfo.subscriberType = model.subscriberType.id
    subscriberInfo.group = model.group
    subscriberInfo.enabled = model.enabled
  }
</script>

<style lang="less" scoped>
  .add-box {
    margin-bottom: 10px;
    text-align: right;
  }
</style>
