import type { PagingFindQuery } from '/@/api/model/baseModel'

export interface sendingOrderQuery extends PagingFindQuery {
  referenceNumbers?: string
  senderName?: string
}

export interface sendingOrderVM {
  id: string
  templateName: string
  SenderName: string
  ServiceName: string
  ReceiveWay: string
  Subject: string
  ContentHeader: string
  Content: string
  ContentFooter: string
  TimerType: string
  SendingOrderStatus: string
  RetryCount: int
  ErrorType: string
  ErrorMessage: string
  updateOn: Date
}
