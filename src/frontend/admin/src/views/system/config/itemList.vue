<template>
	<div class="custom-config-item layout-padding">
		<ProTable v-if="state.isShow" ref="talbeRef" :columns="state.cloumns" :request-api="CustomConfigItemApi.page"></ProTable>
	</div>
</template>

<script setup lang="tsx">
import { ref, reactive, onMounted, nextTick } from 'vue';
import { useRoute } from 'vue-router';
import ProTable from '/@/components/ProTable/index.vue';
import CustomConfigApi from '/@/api/CustomConfigApi';
import CustomConfigItemApi from '/@/api/CustomConfigItemApi';
import { ColumnProps } from '/@/components/ProTable/interface';
const route = useRoute();

// 表格实例
const talbeRef = ref<InstanceType<typeof ProTable>>();

// 页面数据状态
const state = reactive({
	cloumns: [
		{
			type: 'index',
			label: '序号',
			width: 60,
		},
	] as ColumnProps[],
	isShow: false,
});

onMounted(async () => {
	const { data } = await CustomConfigApi.getJson(route.query.id as never);
	const json = JSON.stringify(data!.formJson);
	const reg =
		/{"key":\d+,"type":"(input|select|date|switch|number|textarea|radio|checkbox|time|time-range|date-range|rate|color|slider|cascader|rich-editor|file-upload|picture-upload)".*?"id".*?}/g;
	const optionSting = json.match(reg)?.join(',');
	if (optionSting) {
		const options: Array<any> = JSON.parse(`[${optionSting}]`);
		options
			.filter((i) => i.type !== 'rich-editor')
			.forEach((item) => {
				let option = item.options;
				state.cloumns.push({
					prop: option.name,
					label: option.label,
					// render: (scope) => {
					// 	let row = scope.row;
					// 	let v = row[option.name];
					// 	switch (item.type) {
					// 		case 'select':
					// 		case 'checkbox':
					// 		case 'radio':
					// 			let options = option.optionItems;
					// 			return option.multiple
					// 				? (options as Array<any>)
					// 						.filter((f) => (v as Array<any>).some(f.value))
					// 						.map((m) => m.label)
					// 						.join(',')
					// 				: (options as Array<any>).find((f) => f.value.toString() === v.toString()).label;
					// 		case 'switch':
					// 			let type = option.name.indexOf('启') > -1 || option.name.indexOf('禁') > -1 ? ['启用', '禁用'] : ['是', '否'];
					// 			return <el-switch model-value={row[option.name]} active-text={v ? type[0] : type[1]} active-value={true} inactive-value={false} />;
					// 		case 'time-range':
					// 		case 'date-range':
					// 			return (v as []).join('-');
					// 		default:
					// 			return v;
					// 	}
					// },
				});
			});
	}
	state.cloumns.push({
		prop: 'action',
		label: '操作',
		width: 150,
		fixed: 'right',
	});
	nextTick(() => {
		state.isShow = true;
	});
});
</script>

<style scoped></style>
