<template>
	<div class="system-role-container layout-padding">
		<Search :items="state.search" @search="onSearch" />
		<Table v-bind="state" :on-load="getRolePage" ref="tableRef">
			<template #tools> <el-button type="primary" icon="ele-Plus" @click="onOpenRole(null)"> 新增 </el-button></template>
			<template #status="scope">
				<el-tag :type="scope.row.status === 0 ? 'success' : 'danger'"> {{ scope.row.status === 0 ? '启用' : '禁用' }}</el-tag>
			</template>
			<template #action="scope">
				<el-button icon="ele-Edit" size="small" text type="primary" @click="onOpenRole(scope.row)"> 编辑 </el-button>
				<el-popconfirm title="确认删除吗？" @confirm="onDeleteRole(scope.row.id)">
					<template #reference>
						<el-button icon="ele-Delete" size="small" text type="danger"> 删除 </el-button>
					</template>
				</el-popconfirm>
			</template>
		</Table>
		<RoleDialog ref="roleDialogRef" @refresh="tableRef?.refresh" />
	</div>
</template>

<script setup lang="ts" name="systemRole">
import { defineAsyncComponent, reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import { getRolePage, deleteRole } from '/@/api/SysRoleApi';

// 引入组件
const RoleDialog = defineAsyncComponent(() => import('/@/views/system/role/dialog.vue'));
import Table from '/@/components/table/index.vue';
import Search from '/@/components/table/search.vue';
import { UpdateSysRoleInput } from '/@/api/models';
//  table实例
const tableRef = ref<InstanceType<typeof Table>>();
// 表单实例
const roleDialogRef = ref<InstanceType<typeof RoleDialog>>();
const state = reactive<CustomTable>({
	columns: [
		{
			prop: 'name',
			label: '角色名称',
		},
		{
			prop: 'code',
			label: '角色标识',
			align: 'center',
		},
		{
			prop: 'sort',
			label: '排序',
			align: 'center',
			width: 150,
		},
		{
			prop: 'status',
			label: '状态',
			align: 'center',
			width: 150,
		},
		{
			prop: 'action',
			label: '操作',
			align: 'center',
			width: 150,
		},
	],
	config: {
		isSerialNo: true,
	},
	search: [
		{
			prop: 'name',
			label: '角色名称',
			type: 'input',
		},
	],
});
// 打开新增角色弹窗
const onOpenRole = (row: UpdateSysRoleInput | null) => {
	roleDialogRef.value?.openDialog(row);
};

const onSearch = (data: TableSearchType) => {
	state.param = data;
	tableRef.value?.refresh();
};

// 删除角色
const onDeleteRole = async (id: number) => {
	const { succeeded, errors } = await deleteRole(id);
	if (succeeded) {
		ElMessage.success('删除成功');
		tableRef.value?.refresh();
	} else {
		ElMessage.error(errors);
	}
};
</script>
