import type { AppRouteModule } from '/@/router/types'

import { LAYOUT } from '/@/router/constant'

const dashboard: AppRouteModule = {
  path: '/issuer',
  name: 'Issuer',
  component: LAYOUT,
  redirect: '/issuer/index',
  meta: {
    hideChildrenInMenu: true,
    icon: 'ri:contacts-line',
    //title: t('routes.dashboard.messageSender'),
    title: '消息发起人管理',
    orderNo: 8,
  },
  children: [
    {
      path: 'index',
      name: 'IssuerPage',
      component: () => import('/@/views/issuer/index.vue'),
      meta: {
        title: '消息发起人管理',
        icon: 'simple-icons:about-dot-me',
        hideMenu: true,
      },
    },
  ],
}

export default dashboard
