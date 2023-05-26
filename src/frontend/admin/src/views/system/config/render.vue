<template>
	<div class="layout-padding" v-loading="vm.loading">
		<v-form-render ref="vfRenderRef"> </v-form-render>
		<el-row justify="center">
			<el-button type="primary" size="default" @click="onSubmit">保存</el-button>
		</el-row>
	</div>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { useRoute } from 'vue-router';
import CustomConfigApi from '/@/api/CustomConfigApi';
import { ElMessage } from 'element-plus';
const route = useRoute();
const vfRenderRef = ref();
const vm = reactive({
	loading: true,
	formJson: {},
});
const onSubmit = async () => {
	try {
		const formData = await vfRenderRef.value?.getFormData();
		console.log(formData);
	} catch (e: any) {
		ElMessage.error(e);
	}
};
onMounted(async () => {
	vm.loading = true;
	const id = route.query.id as unknown;
	if (id !== undefined) {
		const { data } = await CustomConfigApi.getJson(id as number);
		if (data !== null) {
			vm.formJson = JSON.parse(data!);
			vfRenderRef.value?.setFormJson(vm.formJson);
		} else {
			ElMessage.error('缺少配置信息');
		}
	} else {
		ElMessage.error('缺少参数');
	}
	vm.loading = false;
});
</script>

<style scoped></style>
