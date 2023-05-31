<template>
	<div class="system-config-container layout-padding">
		<Search :items="state.search" @search="onSearch" />
		<Table v-bind="state" :on-load="CustomConfigApi.page" ref="tableRef">
			<template #tools> <el-button v-auth="'customconfig:add'" type="primary" icon="ele-Plus" @click="onOpenRole(null)"> 新增 </el-button></template>
			<template #status="scope">
				<el-tag :type="scope.row.status === 0 ? 'success' : 'danger'"> {{ scope.row.status === 0 ? '启用' : '禁用' }}</el-tag>
			</template>
			<template #isMultiple="{ row }">
				{{ row.isMultiple ? '多项' : '单项' }}
			</template>
			<template #action="{ row }">
				<el-button v-auth="'customconfig:edit'" icon="ele-Edit" size="small" text type="primary" @click="onOpenRole(row)"> 编辑 </el-button>
				<el-dropdown>
					<el-button icon="ele-MoreFilled" size="small" text type="primary" style="padding-left: 12px" />
					<template #dropdown>
						<el-dropdown-menu>
							<el-dropdown-item icon="ele-List" @click="onConfigItem(row)"> 配置项 </el-dropdown-item>
							<el-dropdown-item icon="ele-BrushFilled" @click="onDesign(row.id)" divided> 配置设计 </el-dropdown-item
							><el-dropdown-item icon="ele-Document" divided @click="onGenerate(row.id)"> 生成实体 </el-dropdown-item>
							<el-dropdown-item icon="ele-Delete" v-auth="'customconfig:delete'" divided @click="onDeleteConfig(row)"> 删除 </el-dropdown-item>
						</el-dropdown-menu>
					</template>
				</el-dropdown>
			</template>
		</Table>
		<ConfigDialog ref="configDialogRef" @refresh="tableRef?.refresh" />
		<RenderDialog ref="renderDialogRef" />
	</div>
</template>

<script setup lang="ts" name="customConfig">
import { defineAsyncComponent, reactive, ref, nextTick } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import CustomConfigApi from '/@/api/CustomConfigApi';
import { auths } from '/@/utils/authFunction';

// 引入组件
const ConfigDialog = defineAsyncComponent(() => import('./configDialog.vue'));
const RenderDialog = defineAsyncComponent(() => import('./renderDialog.vue'));
import Table from '/@/components/table/index.vue';
import Search from '/@/components/table/search.vue';
import { useRouter } from 'vue-router';
import { CustomConfigPageOutput } from '/@/api/models';
const router = useRouter();
//  table实例
const tableRef = ref<InstanceType<typeof Table>>();
// 表单实例
const configDialogRef = ref<InstanceType<typeof ConfigDialog>>();
const renderDialogRef = ref<InstanceType<typeof RenderDialog>>();
//表格列配置
const state = reactive<CustomTable>({
	columns: [
		{
			prop: 'name',
			label: '配置名称',
		},
		{
			prop: 'code',
			label: '唯一编码',
			align: 'center',
		},
		{
			prop: 'status',
			label: '状态',
			align: 'center',
			width: 150,
		},
		{
			prop: 'isMultiple',
			label: '配置类别',
			align: 'center',
			width: 150,
		},
		{
			prop: 'note',
			label: '备注',
			align: 'center',
		},
		{
			prop: 'createdTime',
			label: '创建时间',
			align: 'center',
		},
		{
			prop: 'action',
			label: '操作',
			align: 'center',
			width: 150,
			visible: auths(['customconfig:edit', 'customconfig:delete']),
		},
	],
	config: {
		isSerialNo: true,
	},
	search: [
		{
			prop: 'name',
			label: '配置名称',
			type: 'input',
		},
		{
			prop: 'code',
			label: '唯一编码',
			type: 'input',
		},
	],
});
// 打开新增角色弹窗
const onOpenRole = (row: any) => {
	configDialogRef.value?.openDialog(row, row?.isGenerate ?? false);
};

// 配置表单设计
const onDesign = (id: number) => {
	router.push({ path: '/system/config/design', query: { id } });
};

// 编辑配置项/配置项列表
const onConfigItem = async (row: CustomConfigPageOutput) => {
	await renderDialogRef.value?.openDialog(row.id!);
	//router.push({ path: '/system/config/render', query: { id: row.id } });
};

// 生成配置类
const onGenerate = async (id: number) => {
	await CustomConfigApi.generate(id);
};

// 列表搜索
const onSearch = (data: EmptyObjectType) => {
	state.param = data;
	nextTick(() => {
		tableRef.value?.refresh();
	});
};

// 删除角色
const onDeleteConfig = async (row: CustomConfigPageOutput) => {
	ElMessageBox.confirm(`确定删除配置：【${row.name}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			const { succeeded } = await CustomConfigApi.delete(row.id);
			if (succeeded) {
				ElMessage.success('删除成功');
				tableRef.value?.refresh();
			}
		})
		.catch(() => {});
};
</script>
