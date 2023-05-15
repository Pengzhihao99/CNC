import { PagingFindQuery } from '/@/api/model/baseModel'
export interface SubscriberType {
  id: string
  name: string
}

export interface SubscriberVM {
  id: string
  name: string
  email: string
  phone: string
  enterpriseWeChat: string
  subscriberType: SubscriberType
  group: string
  enabled: boolean
}

export interface AddSubscriber {
  id?: string
  name?: string
  email?: string
  phone?: string
  enterpriseWeChat?: string
  subscriberType: int
  group?: string
  enabled?: boolean
}

export interface UpdateSubscriber {
  id: string
  email?: string
  phone?: string
  enterpriseWeChat?: string
  subscriberType: int
  enabled?: boolean
}

export interface FormStateForSearch {
  group: string
}

export interface SubscriberQuery extends PagingFindQuery {
  group?: string
}
