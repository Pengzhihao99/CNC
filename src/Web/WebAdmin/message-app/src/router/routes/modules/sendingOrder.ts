import type { AppRouteModule } from '/@/router/types'

import { LAYOUT } from '/@/router/constant'

const dashboard: AppRouteModule = {
  path: '/sendingOrder',
  name: 'sendingOrder',
  component: LAYOUT,
  redirect: '/sendingOrder/index',
  meta: {
    hideChildrenInMenu: true,
    icon: 'solar:clipboard-list-linear',
    //title: t('routes.dashboard.messageSender'),
    title: '消息订单发送管理',
    orderNo: 13,
  },
  children: [
    {
      path: 'index',
      name: 'MessageSendingOrderPage',
      component: () => import('/@/views/sendingOrder/index.vue'),
      meta: {
        title: '消息订单发送管理',
        icon: 'solar:clipboard-list-linear',
        hideMenu: true,
      },
    },
  ],
}

export default dashboard
