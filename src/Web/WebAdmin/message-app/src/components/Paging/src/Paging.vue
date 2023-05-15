<template>
  <div class="paging">
    <a-pagination
      show-quick-jumper
      show-size-changer
      v-model:current="props.pageNumber"
      v-model:pageSize="props.pageSize"
      :total="props.total"
      @change="onChange"
      @showSizeChange="onChange"
      :pageSizeOptions="['10', '30', '40']"
      :showTotal="total => `共 ${total} 条数据`"
    />
  </div>
</template>

<script lang="ts" setup>
  const props = defineProps({
    pageNumber: { type: Number, required: true },
    pageSize: { type: Number, required: true },
    total: { type: Number, required: true },
  })

  const emit = defineEmits(['change'])

  const onChange = (currentPageNumber: number, currentPageSize: number) => {
    emit('change', currentPageNumber, currentPageSize)
  }
</script>

<style scoped lang="less">
  .paging {
    margin-top: 10px;
    display: flex;
    justify-content: flex-end;
  }
</style>
