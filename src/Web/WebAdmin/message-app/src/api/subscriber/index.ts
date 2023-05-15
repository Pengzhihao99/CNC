import { defHttp } from '/@/utils/http/axios'
import type { PagingFindResultWrapper } from '/@/api/model/baseModel'
import type {
  AddSubscriber,
  SubscriberQuery,
  SubscriberVM,
  UpdateSubscriber,
} from '/@/api/subscriber/model'

enum Api {
  getSubscriberByPage = '/subscribers',
  AddSubscriber = '/subscribers',
  updateSubscriber = '/subscribers',
}

export const getSubscribersByPage = (params?: SubscriberQuery) =>
  defHttp.get<PagingFindResultWrapper<SubscriberVM>>({
    url: Api.getSubscriberByPage,
    params,
  })

export const addSubscriber = (params: AddSubscriber) =>
  defHttp.post({ url: Api.AddSubscriber, params })

export const updateSubscriber = (params: UpdateSubscriber) =>
  defHttp.put({ url: Api.updateSubscriber, params })
