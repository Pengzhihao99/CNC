import { PagingFindQuery } from '/@/api/model/baseModel'
export interface SendingServiceType {
  id: string
  name: string
}

export interface SendingServiceVM {
  id: string
  serviceName: string
  enabled: boolean
  host: string
  userName: string
  passWord: string
  appKey: string
  appSecret: string
  sendingServiceType: SendingServiceType
  sender: string
  remark: string
}

export interface AddSendingService {
  id?: string
  serviceName: string
  enabled: boolean
  host: string
  userName: string
  passWord: string
  appKey: string
  appSecret: string
  sendingServiceType: int
  sender: string
  remark: string
}

export interface UpdateSendingService {
  id: string
  serviceName: string
  enabled: boolean
  host: string
  userName: string
  passWord: string
  appKey: string
  appSecret: string
  sendingServiceType: int
  sender: string
  remark: string
}

export interface FormStateForSearch {
  serviceName: string
}

export interface SendingServiceQuery extends PagingFindQuery {
  serviceName?: string
}
