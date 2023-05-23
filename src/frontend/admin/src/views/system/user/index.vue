<template>
	<div class="system-user-container layout-padding">
		<Search :items="state.search" @search="onSearch" />
		<Table ref="tableRef" v-bind="state" :on-load="getSysUserPage">
			<template #tools> <el-button type="primary" icon="ele-Plus" @click="onOpenUser(0)"> 新增 </el-button></template>
			<template #status="scope">
				<el-tag :type="scope.row.status === 0 ? 'success' : 'danger'"> {{ scope.row.status === 0 ? '启用' : '禁用' }}</el-tag>
			</template>
			<template #gender="scope">
				<el-tag :type="scope.row.gender === 0 ? '' : scope.row.gender === 1 ? 'success' : 'danger'">
					{{ scope.row.gender === 0 ? '男' : scope.row.gender === 1 ? '女' : '保密' }}</el-tag
				>
			</template>
			<template #action="scope">
				<el-button icon="ele-Edit" size="small" text type="primary" @click="onOpenUser(scope.row.id)"> 编辑 </el-button>
				<el-dropdown>
					<el-button icon="ele-MoreFilled" size="small" text type="primary" style="padding-left: 12px" />
					<template #dropdown>
						<el-dropdown-menu>
							<el-dropdown-item
								icon="ele-RefreshLeft"
								@click="
									() => {
										resetDialogRef?.openDialog(scope.row.id);
									}
								"
							>
								重置密码
							</el-dropdown-item>
							<el-dropdown-item icon="ele-Delete" divided @click="onDeleteUser(scope.row)"> 删除账号 </el-dropdown-item>
						</el-dropdown-menu>
					</template>
				</el-dropdown>
			</template>
		</Table>
		<!-- 用新增编辑 -->
		<UserDialog ref="userDialogRef" @refresh="tableRef?.refresh" />
		<!-- 密码重置 -->
		<ResetDialog ref="resetDialogRef" />
	</div>
</template>

<script setup lang="ts" name="systemUser">
import { defineAsyncComponent, reactive, ref, onMounted, nextTick } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import Table from '/@/components/table/index.vue';
import Search from '/@/components/table/search.vue';
import { getSysUserPage, deleteSysUser } from '/@/api/SysUserApi';
import { getTreeSelect } from '/@/api/SysOrganizationApi';
import { TreeSelectOutput } from '/@/api/models';

// 引入组件
const UserDialog = defineAsyncComponent(() => import('/@/views/system/user/dialog.vue'));
const ResetDialog = defineAsyncComponent(() => import('/@/views/system/user/reset.vue'));

// 表单实例
const userDialogRef = ref<InstanceType<typeof UserDialog>>();
const resetDialogRef = ref<InstanceType<typeof ResetDialog>>();
let orgs = [] as TreeSelectOutput[];
//table实例
const tableRef = ref<InstanceType<typeof Table>>();
const state = reactive<CustomTable>({
	columns: [
		{
			prop: 'account',
			label: '用户名',
			align: 'center',
		},
		{
			prop: 'name',
			label: '姓名',
			align: 'center',
		},
		{
			prop: 'nickName',
			label: '昵称',
			align: 'center',
		},
		{
			prop: 'gender',
			label: '性别',
			align: 'center',
		},
		{
			prop: 'birthday',
			label: '出生日期',
			align: 'center',
		},
		{
			prop: 'mobile',
			label: '手机号码',
			align: 'center',
		},
		{
			prop: 'status',
			label: '状态',
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
			width: 120,
			fixed: 'right',
		},
	],
	config: {
		isSerialNo: true,
	},
	search: [
		{ prop: 'account', label: '用户名', type: 'input' },
		{ prop: 'name', label: '姓名', type: 'input' },
		{ prop: 'mobile', label: '手机号', type: 'input' },
	],
});

// 搜索
const onSearch = (params: EmptyObjectType) => {
	state.param = params;
	nextTick(() => {
		tableRef.value?.refresh();
	});
};

// 打开新增用户弹窗
const onOpenUser = (id: number) => {
	userDialogRef.value?.openDialog(id, orgs);
};
// 删除用户
const onDeleteUser = async (row: any) => {
	ElMessageBox.confirm(`确定删除账号：【${row.account}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			const { succeeded } = await deleteSysUser(row.id);
			if (succeeded) {
				ElMessage.success('删除成功');
				tableRef.value?.refresh();
			}
		})
		.catch(() => {});
};

onMounted(async () => {
	const item = { prop: 'orgId', label: '机构', type: 'treeSelect', options: [] as SelectOptionType[] } as TableSearchType;
	const { data } = await getTreeSelect();
	if ((data ?? []).length > 0) {
		orgs = data!;
		item.options = data! as SelectOptionType[];
		state.search?.push(item);
	}
});
</script>

<style scoped lang="scss"></style>
