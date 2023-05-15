<template>
  <div>
    <div class="search-box">
      <a-form layout="inline" :model="formStateFroSearch">
        <a-form-item>
          <a-textarea
            v-model:value="formStateFroSearch.referenceNumbers"
            placeholder="参考号，多个请用','(逗号)隔开！"
            style="width: 300px"
            :rows="1"
          />
        </a-form-item>
        <a-form-item>
          <a-input
            v-model:value="formStateFroSearch.senderName"
            placeholder="发起人"
            style="width: 200px"
          />
        </a-form-item>
        <a-form-item>
          <a-button type="primary" @click.prevent="handleSearch">查询</a-button>
        </a-form-item>
      </a-form>
    </div>

    <div class="add-box"> </div>

    <a-table
      :defaultExpandAllRows="true"
      :dataSource="dataSource"
      :columns="columns"
      rowKey="id"
      :pagination="false"
      :loading="loading"
    >
      <template #sendingOrderStatus="{ text }">
        <span v-if="text === 'Success'">
          <CheckOutlined :style="{ fontSize: '20px', color: '#08c' }" />
        </span>
        <span v-else-if="text === 'Fail'">
          <CloseOutlined :style="{ fontSize: '20px', color: 'red' }" />
        </span>
        <span v-else-if="text === 'Ready'">
          <QuestionCircleOutlined :style="{ fontSize: '20px', color: 'red' }" />
        </span>
        <span v-else-if="text === 'Sending'">
          <LoadingOutlined :style="{ fontSize: '20px', color: 'green' }" />
        </span>
      </template>

      <template #updateOn="{ text }">
        {{ moment(text).format('YYYY-MM-DD HH:mm:ss') }}
      </template>

      <template #messageContent="{ record }">
        <p>
          {{ record.subject }}
        </p>

        <p>
          {{ record.content }}
        </p>
        <p>
          {{ record.contentFooter }}
        </p>
        <p>
          {{ record.content }}
        </p>
      </template>

      <template #errorMessage="{ record }">
        <p>
          {{ record.errorType }}
        </p>
        {{ record.errorMessage }}
      </template>
    </a-table>
    <Paging :page-number="pageNumber" :page-size="pageSize" :total="total" @change="onChange" />
  </div>
</template>
<script lang="ts" setup>
  import {
    CheckOutlined,
    CloseOutlined,
    LoadingOutlined,
    QuestionCircleOutlined,
  } from '@ant-design/icons-vue'
  import type { Column } from '/#/antd/table'
  import { ref, UnwrapRef, reactive } from 'vue'
  import type { sendingOrderVM } from '/@/api/sendingOrder/model'
  import { getSendingOrderQueryByPage } from '/@/api/sendingOrder'
  import { Paging } from '/@/components/Paging'
  import moment from 'moment'

  import _ from 'lodash'
  const columns: Column[] = [
    {
      title: '参考号',
      dataIndex: 'referenceNumber',
      key: 'referenceNumber',
    },
    {
      title: '模板名',
      dataIndex: 'templateName',
      key: 'templateName',
    },
    {
      title: '服务名',
      dataIndex: 'serviceName',
      key: 'serviceName',
    },
    {
      title: '发起人',
      dataIndex: 'senderName',
      key: 'senderName',
    },
    {
      title: '收信方式',
      dataIndex: 'receiveWay',
      key: 'receiveWay',
    },
    // {
    //   title: '信息内容',
    //   dataIndex: 'messageContent',
    //   key: 'messageContent',
    //   slots: { customRender: 'messageContent' },
    // },

    {
      title: '状态',
      dataIndex: 'sendingOrderStatus',
      key: 'sendingOrderStatus',
      slots: { customRender: 'sendingOrderStatus' },
    },
    {
      title: '定时器',
      dataIndex: 'timerType',
      key: 'timerType',
    },
    {
      title: '更新时间',
      dataIndex: 'updateOn',
      key: 'updateOn',
      slots: { customRender: 'updateOn' },
    },
    {
      title: '重试次数',
      dataIndex: 'retryCount',
      key: 'retryCount',
    },
    {
      title: '错误信息',
      dataIndex: 'errorMessage',
      key: 'errorMessage',
      slots: { customRender: 'errorMessage' },
    },
    // {
    //   title: '操作',
    //   key: 'action',
    //   slots: { customRender: 'action' },
    // },
  ]

  //table
  interface FormStateFroSearch {
    referenceNumbers: string
    senderName: string
  }
  const formStateFroSearch: UnwrapRef<FormStateFroSearch> = reactive({
    referenceNumbers: '',
    senderName: '',
  })

  const loading = ref<Boolean>(true)
  const dataSource = ref<sendingOrderVM[]>([])
  const pageSize = ref<number>(10)
  const pageNumber = ref<number>(1)
  const total = ref<number>(1)
  const fetchData = () => {
    // console.log(
    //   !formStateFroSearch.referenceNumbers
    //     ? []
    //     : _.split(_.replace(formStateFroSearch.referenceNumbers, /\n/g, ','), ',')
    // )
    loading.value = true
    getSendingOrderQueryByPage({
      pageSize: pageSize.value,
      pageNumber: pageNumber.value,
      paging: true,
      referenceNumbers: formStateFroSearch.referenceNumbers,
      senderName: formStateFroSearch.senderName,
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
</script>

<style lang="less" scoped>
  .search-box {
    margin-bottom: 20px;
    text-align: right;
  }
</style>
