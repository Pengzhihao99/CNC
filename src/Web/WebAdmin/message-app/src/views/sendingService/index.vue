<template>
  <div>
    <div class="search-box">
      <a-form layout="inline" :model="formStateForSearch">
        <a-form-item>
          <a-input
            v-model:value="formStateForSearch.serviceName"
            allowClear="true"
            placeholder="消息服务名称"
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
          :model="sendingServiceInfo"
          :rules="rules"
          :label-col="{ span: 4 }"
          :wrapper-col="{ span: 14 }"
        >
          <a-form-item has-feedback label="消息服务名称" ref="serviceName" name="serviceName">
            <a-input
              v-model:value="sendingServiceInfo.serviceName"
              :disabled="modelTitle !== OperationEnum.ADD"
            />
          </a-form-item>
          <a-form-item has-feedback label="发送服务类型" name="sendingServiceType">
            <a-select
              v-model:value="sendingServiceInfo.sendingServiceType"
              placeholder="Please select"
              :options="SendingServiceTypeOptions"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item label="是否启用">
            <a-switch
              v-model:checked="sendingServiceInfo.enabled"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item has-feedback label="Host" name="host">
            <a-input
              v-model:value="sendingServiceInfo.host"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item has-feedback label="账号" name="userName">
            <a-input
              v-model:value="sendingServiceInfo.userName"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item has-feedback label="密码" name="passWord">
            <a-input
              v-model:value="sendingServiceInfo.passWord"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item
            has-feedback
            label="AppKey"
            name="appKey"
            v-if:="sendingServiceInfo.sendingServiceType !== 2"
          >
            <a-input
              v-model:value="sendingServiceInfo.appKey"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item
            has-feedback
            label="AppSecret"
            name="appSecret"
            v-if:="sendingServiceInfo.sendingServiceType !== 2"
          >
            <a-input
              v-model:value="sendingServiceInfo.appSecret"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item has-feedback label="发件人" name="sender">
            <a-input
              v-model:value="sendingServiceInfo.sender"
              :disabled="modelTitle !== OperationEnum.ADD && modelTitle !== OperationEnum.UPDATE"
            />
          </a-form-item>
          <a-form-item has-feedback label="备注" name="remark">
            <a-input
              v-model:value="sendingServiceInfo.remark"
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
  import {
    addSendingService,
    getSendingServicesByPage,
    updateSendingService,
  } from '/@/api/sendingService'
  import {
    AddSendingService,
    SendingServiceVM,
    FormStateForSearch,
  } from '/@/api/sendingService/model'

  import { OperationEnum } from '/@/enums/appEnum'
  import { SendingServiceTypeEnum } from '/@/enums/sendingServiceTypeEnum'
  import { message } from 'ant-design-vue'
  import { SelectTypes } from 'ant-design-vue/lib/select'
  import { ValidateErrorEntity } from 'ant-design-vue/lib/form/interface'
  import { CreateTemplateCommand } from '/@/api/templates/model'

  const columns = [
    {
      title: '消息服务名称',
      dataIndex: 'serviceName',
      sorter: true,
      width: '20%',
      slots: { customRender: 'serviceName' },
    },
    {
      title: '发送服务类型',
      dataIndex: 'sendingServiceType.name',
    },
    {
      title: '是否启用',
      dataIndex: 'enabled',
      slots: { customRender: 'enabled' },
    },
    {
      title: '发件人',
      dataIndex: 'sender',
      slots: { customRender: 'sender' },
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
  const dataSource = ref<SendingServiceVM[]>([])
  const pageSize = ref<number>(10)
  const pageNumber = ref<number>(1)
  const total = ref<number>(1)
  const loading = ref<Boolean>(true)
  const formStateForSearch: UnwrapRef<FormStateForSearch> = reactive({
    serviceName: '',
  })

  const fetchData = () => {
    loading.value = true
    getSendingServicesByPage({
      pageSize: pageSize.value,
      pageNumber: pageNumber.value,
      paging: true,
      serviceName: formStateForSearch.serviceName,
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
    serviceName: [
      { required: true, message: '请输入消息服务名称！', trigger: 'blur' },
      { min: 1, max: 20, message: '字符长度为1-20！', trigger: 'blur' },
    ],
    group: [
      { required: true, message: '请输入从属代码！', trigger: 'blur' },
      { min: 1, max: 20, message: '字符长度为1-20！', trigger: 'blur' },
    ],
  }

  //初始化表单操作对象
  const SendingServiceTypeOptions = ref<SelectTypes['options']>([
    { value: SendingServiceTypeEnum.Phone, label: '手机' },
    { value: SendingServiceTypeEnum.Email, label: '邮箱' },
    { value: SendingServiceTypeEnum.EnterpriseWeChat, label: '企业微信' },
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
        if (!sendingServiceInfo.id) {
          await addSendingService({
            serviceName: sendingServiceInfo.serviceName,
            sendingServiceType: sendingServiceInfo.sendingServiceType,
            enabled: sendingServiceInfo.enabled,
            host: sendingServiceInfo.host,
            userName: sendingServiceInfo.userName,
            passWord: sendingServiceInfo.passWord,
            appKey: sendingServiceInfo.appKey,
            appSecret: sendingServiceInfo.appSecret,
            sender: sendingServiceInfo.sender,
            remark: sendingServiceInfo.remark,
          }).then(() => {
            visible.value = false
            message.success('服务数据添加成功!')
          })
        } else {
          await updateSendingService({
            id: sendingServiceInfo.id,
            serviceName: sendingServiceInfo.serviceName,
            sendingServiceType: sendingServiceInfo.sendingServiceType,
            enabled: sendingServiceInfo.enabled,
            host: sendingServiceInfo.host,
            userName: sendingServiceInfo.userName,
            passWord: sendingServiceInfo.passWord,
            appKey: sendingServiceInfo.appKey,
            appSecret: sendingServiceInfo.appSecret,
            sender: sendingServiceInfo.sender,
            remark: sendingServiceInfo.remark,
          }).then(() => {
            visible.value = false
            message.success('服务数据更新成功!')
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

  const sendingServiceInfo: UnwrapRef<AddSendingService> = reactive({
    id: '',
    serviceName: '',
    sendingServiceType: '',
    enabled: false,
    host: '',
    userName: '',
    passWord: '',
    appKey: '',
    appSecret: '',
    sender: '',
    remark: '',
  })

  const handleAdd = () => {
    isEditing.value = false
    modelTitle.value = OperationEnum.ADD
    visible.value = true

    sendingServiceInfo.id = ''
    sendingServiceInfo.serviceName = ''
    sendingServiceInfo.sendingServiceType = SendingServiceTypeEnum.Email
    sendingServiceInfo.enabled = true
    sendingServiceInfo.host = ''
    sendingServiceInfo.userName = ''
    sendingServiceInfo.passWord = ''
    sendingServiceInfo.appKey = ''
    sendingServiceInfo.appSecret = ''
    sendingServiceInfo.sender = ''
    sendingServiceInfo.remark = ''
  }

  const handleEdit = (model: SendingServiceVM) => {
    isEditing.value = false
    modelTitle.value = OperationEnum.UPDATE
    visible.value = true

    sendingServiceInfo.id = model.id
    sendingServiceInfo.serviceName = model.serviceName
    sendingServiceInfo.sendingServiceType = model.sendingServiceType.id
    sendingServiceInfo.enabled = model.enabled
    sendingServiceInfo.host = model.host
    sendingServiceInfo.userName = model.userName
    sendingServiceInfo.passWord = model.passWord
    sendingServiceInfo.appKey = model.appKey
    sendingServiceInfo.appSecret = model.appSecret
    sendingServiceInfo.sender = model.sender
    sendingServiceInfo.remark = model.remark
  }

  const handleView = (model: SendingServiceVM) => {
    isEditing.value = true
    modelTitle.value = OperationEnum.VIEW
    visible.value = true

    sendingServiceInfo.id = model.id
    sendingServiceInfo.serviceName = model.serviceName
    sendingServiceInfo.sendingServiceType = model.sendingServiceType.id
    sendingServiceInfo.enabled = model.enabled
    sendingServiceInfo.host = model.host
    sendingServiceInfo.userName = model.userName
    sendingServiceInfo.passWord = model.passWord
    sendingServiceInfo.appKey = model.appKey
    sendingServiceInfo.appSecret = model.appSecret
    sendingServiceInfo.sender = model.sender
    sendingServiceInfo.remark = model.remark
  }
</script>

<style lang="less" scoped>
  .add-box {
    margin-bottom: 10px;
    text-align: right;
  }
</style>
