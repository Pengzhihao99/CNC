import type { AppRouteModule } from '/@/router/types'

import { LAYOUT } from '/@/router/constant'

const dashboard: AppRouteModule = {
  path: '/subscriber',
  name: 'Subscriber',
  component: LAYOUT,
  redirect: '/subscriber/index',
  meta: {
    hideChildrenInMenu: true,
    icon: 'icomoon-free:address-book',
    //title: t('routes.dashboard.messageSender'),
    title: '消息订阅者管理',
    orderNo: 6,
  },
  children: [
    {
      path: 'index',
      name: 'MessageSubscriberPage',
      component: () => import('/@/views/subscriber/index.vue'),
      meta: {
        title: '消息订阅者管理',
        icon: 'simple-icons:about-dot-me',
        hideMenu: true,
      },
    },
  ],
}

export default dashboard
