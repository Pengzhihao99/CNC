<template>
  <div>
    <div class="search-box">
      <a-form layout="inline" :model="formStateForSearch">
        <a-form-item>
          <a-input
            v-model:value="formStateForSearch.templateName"
            allowClear="true"
            placeholder="模板名称"
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
          <!-- <a-button type="link" @click="handleView(record)">{{ OperationEnum.VIEW }}</a-button>
          <a-divider type="vertical" /> -->
          <a-button type="link" @click="handleEdit(record)">{{ OperationEnum.UPDATE }}</a-button>
        </span>
      </template>
    </a-table>
    <Paging :page-number="pageNumber" :page-size="pageSize" :total="total" @change="onChange" />

    <a-modal
      v-model:visible="visible"
      width="1000px"
      keyboard="true"
      :title="modelTitle"
      @ok="handleOk"
      @cancel="resetForm"
    >
      <br />
      <a-form
        ref="formRef"
        :model="templateInfo"
        :rules="rules"
        :label-col="{ span: 5 }"
        :wrapper-col="{ span: 14 }"
        layout="horizontal"
      >
        <a-form-item label="模板名" name="templateName" has-feedback>
          <a-input
            v-model:value="templateInfo.templateName"
            :disabled="modelTitle !== OperationEnum.ADD"
          />
        </a-form-item>
        <a-form-item label="备注" name="remark">
          <a-textarea v-model:value="templateInfo.remark" type="textarea" />
        </a-form-item>
        <a-form-item label="是否启用">
          <a-switch v-model:checked="templateInfo.enabled" />
        </a-form-item>
        <a-form-item label="定时器" name="timerType" required>
          <a-select
            v-model:value="templateInfo.timerType"
            placeholder="Please select"
            :options="timerOptions"
          />
        </a-form-item>
        <a-form-item label="唯一性策略" name="onlyStrategyType" required>
          <a-select
            v-model:value="templateInfo.onlyStrategyType"
            :options="onlyStrategyOptions"
            placeholder="Please select"
          />
        </a-form-item>
        <a-form-item label="信息服务" name="sendingServiceId">
          <a-select
            v-model:value="templateInfo.sendingServiceId"
            show-search
            :options="serviceNamesDataSource"
            :filter-option="filterOption"
            placeholder="Please select"
            @change="handleChange"
          />
        </a-form-item>
        <a-divider />
        <div @mouseover="mouseOver" @mouseleave="mouseLeave">
          <a-form-item label="标题" name="templateInfoSubject" has-feedback>
            <a-input
              v-model:value="templateInfo.templateInfoSubject"
              placeholder="e.g. {{model.Subject}}"
            />
          </a-form-item>
          <a-form-item label="模板头" name="templateInfoHeader" has-feedback>
            <a-input
              v-model:value="templateInfo.templateInfoHeader"
              placeholder="e.g. {{model.Header}}"
            />
          </a-form-item>
          <a-form-item v-if="needRichBox" label="模板内容" name="templateInfoContent" has-feedback>
            <Tinymce
              v-model="templateInfo.templateInfoContent"
              width="100%"
              :showImageUpload="false"
            />
          </a-form-item>
          <a-form-item v-else label="模板内容" name="templateInfoContent" has-feedback>
            <a-textarea
              v-model:value="templateInfo.templateInfoContent"
              type="textarea"
              :rows="2"
              placeholder="e.g. {{model.Content}}"
            />
          </a-form-item>

          <a-form-item label="模板尾" name="templateInfoFooter" has-feedback>
            <a-input
              v-model:value="templateInfo.templateInfoFooter"
              placeholder="e.g. {{model.Footer}}"
            />
          </a-form-item>
          <a-form-item label="测试模板内容">
            <a-textarea
              v-model:value="templateInfo.templateInfoFieldValue"
              type="textarea"
              :rows="4"
              placeholder='e.g. {"Subject":"Subject Data","Content":"Content Data"}'
            />
          </a-form-item>
        </div>
        <a-form-item :wrapper-col="{ span: 14, offset: 5 }">
          <span style="float: left">
            <a-alert message="测试通过！" type="success" v-if="successSubShow" show-icon>
              <template #icon><smile-outlined /></template>
            </a-alert>
            <a-alert message="测试失败！" type="error" v-if="errorSubShow" show-icon>
              <template #icon><smile-outlined /></template>
            </a-alert>
          </span>
          <span style="float: right">
            <a-button type="success" size="middle" @click.prevent="onTest">测试模板</a-button>
          </span>
        </a-form-item>

        <a-divider />
        <a-form-item label="发起人" name="issuerNames" has-feedback>
          <a-textarea
            v-model:value="templateInfo.issuerNames"
            type="textarea"
            :rows="4"
            placeholder="请点击发起人列表进行添加操作。"
            disabled="true"
          />
          <br />
          <a-button @click="getReceiver" shape="circle" type="text" style="float: right">
            <UsergroupAddOutlined />
          </a-button>
        </a-form-item>
      </a-form>
    </a-modal>

    <a-drawer
      title="模板展示"
      placement="right"
      :closable="true"
      v-model:visible="TestVisible"
      width="30%"
      @close="issuerHandleClose"
    >
      <h1 style="font-size: x-large">主题：{{ subject }}</h1>
      <div v-html="htmlPage"></div>
    </a-drawer>
    <a-modal
      v-model:visible="issuerVisible"
      width="1000px"
      keyboard="true"
      title="发起人选择"
      @ok="issuerHandleOk"
    >
      <div style="width: 40%; margin: 15px 0px 15px 10px">
        <a-form layout="inline" :model="issuerForSearch">
          <a-form-item>
            <a-input
              v-model:value="issuerForSearch.name"
              allowClear="true"
              placeholder="发起人包含"
            />
          </a-form-item>
          <a-form-item>
            <a-button type="primary" @click.prevent="issuerHandleSearch">查询</a-button>
          </a-form-item>
        </a-form>
      </div>
      <span style="margin-left: 15px">
        <template v-if="hasSelected">
          {{ `共选择 ${state.selectedRowKeys.length} 条数据！` }}
        </template>
      </span>
      <div style="width: 98%; margin: auto">
        <a-table
          :row-selection="{
            selectedRowKeys: state.selectedRowKeys,
            onChange: onSelectChange,
          }"
          :columns="issuerColumns"
          :data-source="issuerData"
          :pagination="false"
          row-key="id"
          row-text="name"
        />
        <Paging
          :page-number="issuerPageNumber"
          :page-size="issuerPageSize"
          :total="issuerTotal"
          @change="issuerOnChange"
        />
        <br />
      </div>
    </a-modal>
  </div>
</template>
<script lang="ts" setup>
  import {
    UsergroupAddOutlined,
    SmileOutlined,
    CheckOutlined,
    CloseOutlined,
  } from '@ant-design/icons-vue'
  import { reactive, ref, UnwrapRef, computed } from 'vue'
  import moment from 'moment'
  import { ValidateErrorEntity } from 'ant-design-vue/es/form/interface'
  import { SelectTypes } from 'ant-design-vue/es/select'
  import {
    TemplateVM,
    SendingServiceNamesVM,
    CreateTemplateCommand,
    FormStateForSearch,
    FromForTemplate,
    DDLDataSource,
  } from '/@/api/templates/model'
  import { IssuerVM } from '/@/api/issuer/model'
  import {
    getTemplateByPage,
    addTemplate,
    updateTemplate,
    testTemplate,
    getSendingServiceNames,
  } from '/@/api/templates'
  import { getIssuerByPage } from '/@/api/issuer'
  import { OperationEnum } from '/@/enums/appEnum'
  import { OnlyStrategyTypeEnum } from '/@/enums/templateEnum'
  import { TimerTypeEnum } from '../../enums/templateEnum'
  import { Paging } from '/@/components/Paging'
  import { Tinymce } from '/@/components/Tinymce/index'
  import { message } from 'ant-design-vue'
  import { useUserStore } from '/@/store/modules/user'
  import { ColumnProps } from 'ant-design-vue/es/table/interface'
  import _ from 'lodash-es'

  const columns = [
    {
      title: '模板名',
      dataIndex: 'templateName',
    },
    {
      title: '消息服务',
      dataIndex: 'sendingServiceName',
    },
    {
      title: '是否启用',
      dataIndex: 'enabled',
      slots: { customRender: 'enabled' },
    },
    {
      title: '定时器',
      dataIndex: 'timerType.name',
    },
    {
      title: '唯一性策略',
      dataIndex: 'onlyStrategyType.name',
    },
    {
      title: '创建人',
      dataIndex: 'creator',
    },
    {
      title: '备注',
      dataIndex: 'remark',
    },
    {
      title: '更新时间',
      dataIndex: 'updateOn',
      slots: { customRender: 'updateOn' },
    },
    {
      title: '操作',
      dataIndex: 'action',
      //使用插槽
      slots: { customRender: 'action' },
    },
  ]

  const issuerColumns = [
    {
      title: '发起人',
      dataIndex: 'name',
      sorter: true,
      width: '20%',
      slots: { customRender: 'name' },
    },
    {
      title: '备注',
      dataIndex: 'remark',
    },
  ]

  //表单校验
  const rules = {
    templateName: [
      { required: true, message: '请输入模板名！', trigger: 'blur' },
      { min: 1, max: 30, message: '字符长度为1-30！', trigger: 'blur' },
      { pattern: /^[^\s]*$/, message: '不允许有空格！', trigger: 'blur' },
    ],
    timerType: [{ required: true, message: '请选择定时器！' }],
    onlyStrategyType: [{ required: true, message: '请选择唯一性策略！' }],
    templateInfoSubject: [{ required: true, message: '请输入标题！', trigger: 'blur' }],
    templateInfoHeader: [{ required: true, message: '请输入模板内容头！', trigger: 'blur' }],
    templateInfoContent: [{ required: true, message: '请输入模板内容！', trigger: 'blur' }],
    templateInfoFooter: [{ required: true, message: '请输入模板内容尾！', trigger: 'blur' }],
    issuerNames: [{ required: true, message: '请添加发起人！', trigger: 'blur' }],
  }

  //初始化表单对象
  const templateInfo: UnwrapRef<FromForTemplate> = reactive({
    id: '',
    creator: '',
    templateName: '',
    enabled: true,
    timerType: '',
    onlyStrategyType: '',
    sendingServiceId: '',
    sendingServiceName: '',
    templateInfoSubject: '',
    templateInfoHeader: '',
    templateInfoContent: '',
    templateInfoFooter: '',
    templateInfoFieldValue: '',
    issuerIds: [],
    issuerNames: '',
    remark: '',
  })
  const dataSource = ref<TemplateVM[]>([])
  const sendingServiceNames = ref<SendingServiceNamesVM[]>([])
  const serviceNamesDataSource = ref<DDLDataSource[]>([])
  const pageSize = ref<number>(10)
  const pageNumber = ref<number>(1)
  const total = ref<number>(1)
  const loading = ref<Boolean>(true)
  const formStateForSearch: UnwrapRef<FormStateForSearch> = reactive({
    templateName: '',
  })

  //查询
  const fetchData = () => {
    loading.value = true
    getTemplateByPage({
      pageSize: pageSize.value,
      pageNumber: pageNumber.value,
      paging: true,
      templateName: formStateForSearch.templateName,
    }).then(rep => {
      dataSource.value = rep.data
      total.value = rep.totalRecords
      pageNumber.value = rep.pageNumber
    })

    getSendingServiceNames().then(rep => {
      sendingServiceNames.value = rep
      loading.value = false
    })
  }

  //search
  const handleServiceNames = () => {
    sendingServiceNames.value.forEach((r: SendingServiceNamesVM) => {
      serviceNamesDataSource.value.push({
        label: r.serviceName,
        value: r.id,
        type: r.sendingServiceType.name,
      })
    })
  }

  //search
  const handleSearch = () => {
    pageNumber.value = 1
    fetchData()
  }
  //显示
  fetchData()
  //分页
  const onChange = (currentPageNumber: number, currentPageSize: number) => {
    pageNumber.value = currentPageNumber
    pageSize.value = currentPageSize
    fetchData()
  }

  //对话框确认
  const handleOk = () => {
    formRef.value
      .validate()
      .then(async () => {
        if (!successSubShow.value) {
          message.warn('请测试模板成功后再添加！')
          return
        }

        //创建
        if (!templateInfo.id) {
          //console.log(templateInfo)
          await addTemplate({
            creator: templateInfo.creator,
            templateName: templateInfo.templateName,
            enabled: templateInfo.enabled,
            timerType: templateInfo.timerType,
            onlyStrategyType: templateInfo.onlyStrategyType,
            sendingServiceId: templateInfo.sendingServiceId,
            sendingServiceName: templateInfo.sendingServiceName,
            templateInfoSubject: templateInfo.templateInfoSubject,
            templateInfoHeader: templateInfo.templateInfoHeader,
            templateInfoContent: templateInfo.templateInfoContent,
            templateInfoFooter: templateInfo.templateInfoFooter,
            templateInfoFieldValue: templateInfo.templateInfoFieldValue,
            issuerIds: templateInfo.issuerIds,
            issuerNames: templateInfo.issuerNames,
            remark: templateInfo.remark,
          }).then(() => {
            visible.value = false
            message.success('数据添加成功！')
          })
        } //更新
        else {
          await updateTemplate({
            id: templateInfo.id,
            creator: templateInfo.creator,
            enabled: templateInfo.enabled,
            timerType: templateInfo.timerType,
            onlyStrategyType: templateInfo.onlyStrategyType,
            sendingServiceId: templateInfo.sendingServiceId,
            sendingServiceName: templateInfo.sendingServiceName,
            templateInfoSubject: templateInfo.templateInfoSubject,
            templateInfoHeader: templateInfo.templateInfoHeader,
            templateInfoContent: templateInfo.templateInfoContent,
            templateInfoFooter: templateInfo.templateInfoFooter,
            templateInfoFieldValue: templateInfo.templateInfoFieldValue,
            issuerIds: templateInfo.issuerIds,
            issuerNames: templateInfo.issuerNames,
            remark: templateInfo.remark,
          }).then(() => {
            visible.value = false
            message.success('数据更新成功！')
          })
        }
        handleSearch()
        resetForm()
      })
      .catch((error: ValidateErrorEntity<CreateTemplateCommand>) => {
        console.log('error', error)
      })
  }

  //对话框取消
  const resetForm = () => {
    formRef.value.resetFields()
  }

  //测试模板按钮
  let htmlPage = ref<string>('')
  let subject = ref<string>('')
  const successSubShow = ref<boolean>(false)
  const errorSubShow = ref<boolean>(false)
  const TestVisible = ref<boolean>(false)
  const onTest = async () => {
    try {
      JSON.parse(templateInfo.templateInfoFieldValue)
    } catch {
      message.warn(
        '测试模板内容填写错误，请填写正确的json格式！如：{"Subject":"Subject Data","Content":"Content Data"}'
      )
      return
    }
    if (
      templateInfo.templateInfoFieldValue == '' ||
      templateInfo.templateInfoFieldValue == 'null'
    ) {
      message.warn(
        '测试模板内容填写错误，请填写正确的json格式！如：{"Subject":"Subject Data","Content":"Content Data"}'
      )
      return
    }

    if (
      templateInfo.templateInfoSubject &&
      templateInfo.templateInfoContent &&
      templateInfo.templateInfoFieldValue
    ) {
      await testTemplate({
        subject: templateInfo.templateInfoSubject,
        header: templateInfo.templateInfoHeader,
        content: templateInfo.templateInfoContent,
        footer: templateInfo.templateInfoFooter,
        fieldValue: JSON.parse(templateInfo.templateInfoFieldValue),
      })
        .then(result => {
          TestVisible.value = true
          subject.value = result.subject
          htmlPage.value = result.htmlPage
          errorSubShow.value = false
          successSubShow.value = true

          changeState.oldArrValue = [
            templateInfo.templateInfoSubject,
            templateInfo.templateInfoHeader,
            templateInfo.templateInfoContent,
            templateInfo.templateInfoFooter,
            templateInfo.templateInfoFieldValue,
          ]
          //htmlPage.value = ''
        })
        .catch(() => {
          successSubShow.value = false
          errorSubShow.value = true
        })
    } else {
      message.warn('标题，模板内容，字段值不能为空！')
    }
  }

  const needRichBox = ref<boolean>(false)
  function handleChange(value: string, option: any) {
    templateInfo.sendingServiceId = value
    templateInfo.sendingServiceName = option.label
    handleRichBox()
  }

  function handleRichBox(): void {
    if (templateInfo.sendingServiceId) {
      try {
        for (let key in serviceNamesDataSource.value) {
          const value = serviceNamesDataSource.value[key]
          if (templateInfo.sendingServiceId == value.value) {
            if (value.type == 'Email') {
              needRichBox.value = true
              break
            } else {
              needRichBox.value = false
            }
          }
        }
      } catch {
        needRichBox.value = false
      }
    }
  }

  function filterOption(input: string, option: any) {
    return option.value.toLowerCase().indexOf(input.toLowerCase()) >= 0
  }

  //发送者选择
  const issuerPageSize = ref<number>(10)
  const issuerPageNumber = ref<number>(1)
  const issuerTotal = ref<number>(1)
  const issuerData = ref<IssuerVM[]>([])
  const issuerVisible = ref<boolean>(false)
  const hasSelected = computed(() => state.selectedRowKeys.length > 0)
  const issuerForSearch = reactive<{
    name: string
  }>({
    name: '',
  })

  const getIssuerData = () => {
    loading.value = true
    getIssuerByPage({
      pageSize: issuerPageSize.value,
      pageNumber: issuerPageNumber.value,
      paging: true,
      name: issuerForSearch.name,
    }).then(rep => {
      issuerData.value = rep.data
      issuerTotal.value = rep.totalRecords
      issuerPageNumber.value = rep.pageNumber
      loading.value = false
    })
  }
  const getReceiver = () => {
    issuerVisible.value = true
    issuerPageNumber.value = 1
    getIssuerData()
  }

  const issuerOnChange = (currentPageNumber: number, currentPageSize: number) => {
    issuerPageNumber.value = currentPageNumber
    issuerPageSize.value = currentPageSize
    getIssuerData()
  }

  const issuerHandleSearch = () => {
    issuerPageNumber.value = 1
    getIssuerData()
  }

  type Key = ColumnProps['key']
  interface IssuerArr {
    id: string
    name: string
  }
  const state = reactive<{
    selectedRowKeys: Key[]
    selectedRows: IssuerArr[]
  }>({
    selectedRowKeys: [],
    selectedRows: [],
  })

  const onSelectChange = (selectedRowKeys: Key[], selectedRows: IssuerArr[]) => {
    state.selectedRowKeys = selectedRowKeys
    let allRows = state.selectedRows.concat(selectedRows)
    let curRows = allRows.filter(item => {
      return selectedRowKeys.includes(item.id)
    })
    let filtered = curRows.reduce((total, next) => {
      let has = total.some(item => {
        return item.id === next.id
      })
      return has ? total : total.concat([next]) //连接数组
    }, [])
    state.selectedRows = filtered

    // console.log(state.selectedRowKeys)
    // console.log(state.selectedRows)
  }

  const issuerHandleOk = () => {
    templateInfo.issuerNames = ''
    templateInfo.issuerIds = state.selectedRowKeys
    state.selectedRows.forEach(item => {
      templateInfo.issuerNames += item.name + ','
    })
    //去掉最后一个逗号
    templateInfo.issuerNames = templateInfo.issuerNames.slice(
      0,
      templateInfo.issuerNames.length - 1
    )
    issuerVisible.value = false
  }
  const issuerHandleClose = () => {
    htmlPage.value = ''
  }

  //初始化表单操作对象
  const formRef = ref()
  const visible = ref<boolean>(false)
  const modelTitle = ref<String>('')
  const creator = useUserStore().getUserInfo?.realName
  const timerOptions = ref<SelectTypes['options']>([
    {
      value: TimerTypeEnum.Immediately,
      label: '立即',
    },
    {
      value: TimerTypeEnum.Employee,
      label: '间隔1小时',
    },
  ])
  const onlyStrategyOptions = ref<SelectTypes['options']>([
    {
      value: OnlyStrategyTypeEnum.TwentyFourHour,
      label: '24小时',
    },
    {
      value: OnlyStrategyTypeEnum.TwelveHour,
      label: '12小时',
    },
    {
      value: OnlyStrategyTypeEnum.None,
      label: '无',
    },
  ])

  //添加
  const changeState = reactive<{
    newArrValue: string[]
    oldArrValue: string[]
  }>({
    newArrValue: [''],
    oldArrValue: [''],
  })

  const handleAdd = () => {
    modelTitle.value = OperationEnum.ADD
    visible.value = true
    successSubShow.value = false
    errorSubShow.value = false
    templateInfo.id = ''
    templateInfo.creator = creator
    templateInfo.enabled = true
    templateInfo.templateName = ''
    templateInfo.timerType = TimerTypeEnum.Immediately
    templateInfo.onlyStrategyType = OnlyStrategyTypeEnum.TwentyFourHour
    templateInfo.sendingServiceId = ''
    templateInfo.sendingServiceName = ''
    templateInfo.templateInfoSubject = ''
    templateInfo.templateInfoHeader = ''
    templateInfo.templateInfoContent = ''
    templateInfo.templateInfoFooter = ''
    templateInfo.templateInfoFieldValue = ''
    templateInfo.issuerIds = []
    templateInfo.issuerNames = ''
    templateInfo.remark = ''

    state.selectedRowKeys = []
    state.selectedRows = []
    serviceNamesDataSource.value = []
    handleServiceNames()
  }
  // 编辑
  const handleEdit = (model: TemplateVM) => {
    //console.log(model)
    modelTitle.value = OperationEnum.UPDATE
    visible.value = true
    successSubShow.value = true
    errorSubShow.value = false
    templateInfo.id = model.id
    templateInfo.creator = creator
    templateInfo.enabled = model.enabled
    templateInfo.templateName = model.templateName
    templateInfo.timerType = model.timerType.id
    templateInfo.onlyStrategyType = model.onlyStrategyType.id
    templateInfo.sendingServiceId = model.sendingServiceId
    templateInfo.sendingServiceName = model.sendingServiceName
    templateInfo.templateInfoSubject = model.templateInfo.subject
    templateInfo.templateInfoHeader = model.templateInfo.header
    templateInfo.templateInfoContent = model.templateInfo.content
    templateInfo.templateInfoFooter = model.templateInfo.footer
    templateInfo.templateInfoFieldValue = model.templateInfo.fieldValue
    templateInfo.issuerIds = model.issuerIds
    templateInfo.issuerNames = model.issuerNames
    templateInfo.remark = model.remark
    state.selectedRowKeys = model.issuerIds

    htmlPage.value = ''
    changeState.oldArrValue = [
      model.templateInfo.subject,
      model.templateInfo.header,
      model.templateInfo.content,
      model.templateInfo.footer,
      model.templateInfo.fieldValue,
    ]
    //let i = 1
    // let startInfo = ['']
    // watch(
    //   [
    //     () => templateInfo.templateInfoSubject,
    //     () => templateInfo.templateInfoHeader,
    //     () => templateInfo.templateInfoContent,
    //     () => templateInfo.templateInfoFooter,
    //     () => templateInfo.templateInfoFieldValue,
    //   ],
    //   (newValue, oldValue) => {
    //     console.log(i)
    //     if (i == 1) {
    //       startInfo = oldValue
    //     }
    //     i++
    //     let p = newValue.every(item => {
    //       return startInfo.indexOf(item) !== -1
    //     })
    //     console.log(p)
    //     if (!p) {
    //       successSubShow.value = false
    //       errorSubShow.value = true
    //     } else {
    //       errorSubShow.value = false
    //       successSubShow.value = true
    //     }
    //   }
    // )
    serviceNamesDataSource.value = []
    handleServiceNames()
    handleRichBox()
  }
  // 查看
  const mouseOver = () => {}

  const mouseLeave = () => {
    changeState.newArrValue = [
      templateInfo.templateInfoSubject,
      templateInfo.templateInfoHeader,
      templateInfo.templateInfoContent,
      templateInfo.templateInfoFooter,
      templateInfo.templateInfoFieldValue,
    ]
    let b = changeState.newArrValue.every(item => {
      return changeState.oldArrValue.indexOf(item) !== -1
    })
    if (!htmlPage.value && modelTitle.value == OperationEnum.UPDATE) {
      if (!b) {
        errorSubShow.value = true
        successSubShow.value = false
        //message.success('检测到模板有更改，请重新测试！')
      } else {
        errorSubShow.value = false
        successSubShow.value = true
      }
    }
  }
</script>
<style lang="less" scoped>
  .add-box {
    margin-bottom: 10px;
    text-align: right;
  }
</style>
