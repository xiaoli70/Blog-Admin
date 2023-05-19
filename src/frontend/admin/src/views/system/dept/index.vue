<template>
	<div class="system-dept-container layout-padding">
		<Search :items="state.search" @search="onSearch" />
		<Table ref="tableRef" v-bind="state" :on-load="getSysOrgPage">
			<template #tools>
				<el-button type="primary" icon="ele-Plus" @click="onOpenDept(null)"> 新增 </el-button>
			</template>
			<template #status="scope">
				<el-tag :type="scope.row.status === 0 ? 'success' : 'danger'"> {{ scope.row.status === 0 ? '启用' : '禁用' }}</el-tag>
			</template>
			<template #action="scope">
				<el-button icon="ele-Edit" size="small" text type="primary" @click="onOpenDept(scope.row)"> 编辑 </el-button>
				<el-popconfirm title="确认删除吗？" @confirm="onDeleteOrg(scope.row.id)">
					<template #reference>
						<el-button icon="ele-Delete" size="small" text type="danger"> 删除 </el-button>
					</template>
				</el-popconfirm>
			</template>
		</Table>
		<DeptDialog ref="deptDialogRef" @refresh="tableRef?.refresh()" />
	</div>
</template>

<script setup lang="ts" name="systemDept">
import { defineAsyncComponent, ref, reactive } from 'vue';
import { ElMessage } from 'element-plus';

// 引入组件
const DeptDialog = defineAsyncComponent(() => import('/@/views/system/dept/dialog.vue'));
import Search from '/@/components/table/search.vue';
import Table from '/@/components/table/index.vue';
import { getSysOrgPage, deleteOrg } from '/@/api/SysOrganizationApi';
import type { UpdateOrgInput } from '/@/api/models';

// 定义变量内容
const deptDialogRef = ref<InstanceType<typeof DeptDialog>>();
const tableRef = ref<InstanceType<typeof Table>>();
const state = reactive<CustomTable>({
	columns: [
		{
			prop: 'name',
			label: '机构名称',
		},
		{
			prop: 'code',
			label: '机构编码',
			align: 'center',
		},
		{
			prop: 'status',
			label: '状态',
			align: 'center',
		},
		{
			prop: 'sort',
			label: '排序',
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
		},
	],
	search: [{ label: '机构名称', prop: 'name', placeholder: '机构名称', type: 'input' }],
	config: {
		isSerialNo: false,
	},
});

const onSearch = (data: TableSearchType) => {
	state.param = data;
	tableRef.value?.refresh();
};

// 打开新增菜单弹窗
const onOpenDept = async (row: UpdateOrgInput | null = null) => {
	await deptDialogRef.value?.openDialog(row);
};

//删除机构
const onDeleteOrg = async (id: number) => {
	const { succeeded, errors } = await deleteOrg(id);
	if (succeeded) {
		ElMessage.success('删除成功');
		tableRef.value?.refresh();
	} else {
		ElMessage.error(errors);
	}
};
</script>
