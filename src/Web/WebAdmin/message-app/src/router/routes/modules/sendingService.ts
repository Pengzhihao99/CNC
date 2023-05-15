import type { AppRouteModule } from '/@/router/types'

import { LAYOUT } from '/@/router/constant'

const dashboard: AppRouteModule = {
  path: '/sendingService',
  name: 'sendingService',
  component: LAYOUT,
  redirect: '/sendingService/index',
  meta: {
    hideChildrenInMenu: true,
    icon: 'icomoon-free:sphere',
    //title: t('routes.dashboard.messageSender'),
    title: '消息服务管理',
    orderNo: 2,
  },
  children: [
    {
      path: 'index',
      name: 'MessageSendingServicePage',
      component: () => import('/@/views/sendingService/index.vue'),
      meta: {
        title: '消息服务管理',
        icon: 'simple-icons:about-dot-me',
        hideMenu: true,
      },
    },
  ],
}

export default dashboard
