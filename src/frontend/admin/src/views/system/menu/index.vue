<template>
	<div class="system-menu-container layout-padding">
		<Search :items="vm.search" @search="onSearch" />
		<Table ref="tableRef" v-bind="vm" :on-load="getMenuPage" :tree-props="{ children: 'children', hasChildren: 'hasChildren' }">
			<template #tools>
				<el-button type="primary" icon="ele-Plus" @click="onOpenMenu"> 新增 </el-button>
			</template>
			<template #name="scope">
				<SvgIcon :name="scope.row.icon" />
				<span class="ml10">{{ $t(scope.row.name) }}</span>
			</template>
			<template #type="scope">
				<el-tag :type="scope.row.type === 0 ? '' : scope.row.type === 1 ? 'success' : 'danger'">
					{{ scope.row.type === 0 ? '目录' : scope.row.type === 1 ? '菜单' : '按钮' }}</el-tag
				>
			</template>
			<template #status="scope">
				<el-tag :type="scope.row.status === 0 ? 'success' : 'danger'"> {{ scope.row.status === 0 ? '启用' : '禁用' }}</el-tag>
			</template>
			<template #action="scope">
				<el-button v-auth="'sysmenu:edit'" icon="ele-Edit" size="small" text type="primary" @click="onOpenMenu(scope.row.id)"> 编辑 </el-button>
				<el-popconfirm v-auth="'sysmenu:delete'" title="确认删除吗？" @confirm="onDeleteMenu(scope.row.id)">
					<template #reference>
						<el-button icon="ele-Delete" size="small" text type="danger"> 删除 </el-button>
					</template>
				</el-popconfirm>
			</template>
		</Table>
		<MenuDialog ref="menuDialogRef" @refresh="tableRef?.refresh()" />
	</div>
</template>

<script setup lang="ts" name="systemMenu">
import { defineAsyncComponent, ref, reactive, nextTick } from 'vue';
import { ElMessage } from 'element-plus';
import { getMenuPage, deleteMenu } from '/@/api/SysMenuApi';
import { auths } from '/@/utils/authFunction';
import Search from '/@/components/table/search.vue';
import Table from '/@/components/table/index.vue';
import { onMounted } from 'vue';

// 引入组件
const MenuDialog = defineAsyncComponent(() => import('/@/views/system/menu/dialog.vue'));
//table组件实例
const tableRef = ref<InstanceType<typeof Table>>();
const vm = reactive<CustomTable>({
	columns: [
		{ prop: 'name', label: '菜单名称', align: 'left' },
		{ prop: 'type', label: '类型', align: 'center' },
		{ prop: 'path', label: '路由地址', align: 'center' },
		{ prop: 'component', label: '组件路径', align: 'center' },
		{ prop: 'code', label: '权限标识', align: 'center' },
		{ prop: 'status', label: '状态', align: 'center' },
		{ prop: 'sort', label: '排序', align: 'center' },
		{ prop: 'createdTime', label: '创建时间', align: 'center' },
		{ prop: 'action', label: '操作', align: 'center', width: 150, fixed: 'right' },
	],
	search: [{ label: '名称', prop: 'name', placeholder: '菜单按钮名称', type: 'input' }],
	config: {
		isSerialNo: false,
	},
});

// 定义变量内容
const menuDialogRef = ref<InstanceType<typeof MenuDialog>>();

//查询
const onSearch = (data: EmptyObjectType) => {
	vm.param = data;
	nextTick(() => {
		tableRef.value?.refresh();
	});
};

// 打开添加编辑菜单弹窗
const onOpenMenu = async (id: number = 0) => {
	await menuDialogRef.value!.openDialog(id);
};

//删除菜单
const onDeleteMenu = async (id: number) => {
	const { succeeded, errors } = await deleteMenu(id);
	if (succeeded) {
		ElMessage.success('删除成功');
		tableRef.value?.refresh();
	} else {
		ElMessage.error(errors);
	}
};

onMounted(() => {
	if (!auths(['sysmenu:edit', 'sysmenu:delete'])) {
		vm.columns = vm.columns.filter((item) => item.prop !== 'action');
	}
});
</script>
