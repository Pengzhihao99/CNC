import { defHttp } from '/@/utils/http/axios'
import type { sendingOrderQuery, sendingOrderVM } from '/@/api/sendingOrder/model'
import type { PagingFindResultWrapper } from '/@/api/model/baseModel'

enum Api {
  mainSendingOrderApi = '/sendingOrders',
}

export const getSendingOrderQueryByPage = (params?: sendingOrderQuery) =>
  defHttp.get<PagingFindResultWrapper<sendingOrderVM>>({
    url: Api.mainSendingOrderApi,
    params,
  })
