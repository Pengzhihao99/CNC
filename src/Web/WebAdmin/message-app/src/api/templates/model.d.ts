import { PagingFindQuery } from '/@/api/model/baseModel'

export interface TimerType {
  id: string
  name: string
}
export interface OnlyStrategyType {
  id: string
  name: string
}
export interface SubscriberType {
  id: string
  name: string
}
export interface TemplateVM {
  id: string
  creator: string
  templateName: string
  enabled: boolean
  timerType: TimerType
  onlyStrategyType: OnlyStrategyType
  sendingServiceId: string
  sendingServiceName: string
  templateInfo: TemplateInfo
  issuerIds: (string | number | undefined)[]
  issuerNames: string
  remark: string
  updateOn: string
}

export interface DDLDataSource {
  label: string
  value: string
  type: string
}

export interface SendingServiceNamesVM {
  id: string
  serviceName: string
  sendingServiceType: SendingServiceType
}

export interface SendingServiceType {
  id: string
  name: string
}

export interface TemplateInfo {
  subject: string
  header: string
  content: string
  footer: string
  fieldValue: string
}

export interface TemplateQuery extends PagingFindQuery {
  templateName: string
}

export interface FormStateForSearch {
  templateName: string
}

export interface SendingService {
  value: string
  label: string
  type: string
}

export interface TestTemplateCommand {
  subject: string
  header: string
  content: string
  footer: string
  fieldValue: string
}

export interface FromForTemplate {
  id: string
  creator: string
  templateName: string
  enabled: boolean
  timerType: int
  onlyStrategyType: int
  sendingServiceId: string
  sendingServiceName: string
  templateInfoSubject: string
  templateInfoHeader: string
  templateInfoContent: string
  templateInfoFooter: string
  templateInfoFieldValue: string
  issuerIds: (string | number | undefined)[]
  issuerNames: string
  remark: string
}

export interface CreateTemplateCommand {
  creator: string
  templateName: string
  enabled: boolean
  timerType: int
  onlyStrategyType: int
  sendingServiceId: string
  sendingServiceName: string
  templateInfoSubject: string
  templateInfoHeader: string
  templateInfoContent: string
  templateInfoFooter: string
  templateInfoFieldValue: string
  issuerIds: (string | number | undefined)[]
  issuerNames: string
  remark: string
}

export interface UpdateTemplateCommand {
  id: string
  creator: string
  //templateName: string
  enabled: boolean
  timerType: int
  onlyStrategyType: int
  sendingServiceId: string
  sendingServiceName: string
  templateInfoSubject: string
  templateInfoHeader: string
  templateInfoContent: string
  templateInfoFooter: string
  templateInfoFieldValue: string
  issuerIds: (string | number | undefined)[]
  issuerNames: string
  remark: string
}
