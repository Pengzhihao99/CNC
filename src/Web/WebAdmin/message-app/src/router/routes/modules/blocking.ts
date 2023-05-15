import type { AppRouteModule } from '/@/router/types'

import { LAYOUT } from '/@/router/constant'

const dashboard: AppRouteModule = {
  path: '/blocking',
  name: 'blocking',
  component: LAYOUT,
  redirect: '/blocking/index',
  meta: {
    hideChildrenInMenu: true,
    icon: 'ri:git-repository-private-line',
    title: '消息拦截管理',
    orderNo: 9,
  },
  children: [
    {
      path: 'index',
      name: 'blockingPage',
      component: () => import('/@/views/blocking/index.vue'),
      meta: {
        title: '消息拦截管理',
        icon: 'simple-icons:about-dot-me',
        hideMenu: true,
      },
    },
  ],
}

export default dashboard
