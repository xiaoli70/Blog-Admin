<template>
	<div class="article-operate-container layout-padding">
		<div class="card">
			<el-form class="form">
				<div class="left">
					<el-row :gutter="20" class="title">
						<el-col :span="19">
							<el-row>
								<el-col class="mb20">
									<el-form-item label="文章标题" prop="title">
										<el-input maxlength="128" v-model="state.form.title" placeholder="请输入文章标题" clearable></el-input>
									</el-form-item>
								</el-col>
								<el-col class="mb20">
									<el-form-item label="内容摘要" prop="summary">
										<el-input
											resize="none"
											v-model="state.form.summary"
											maxlength="256"
											type="textarea"
											placeholder="请输入内容摘要"
											show-word-limit
											clearable
										></el-input>
									</el-form-item>
								</el-col>
							</el-row>
						</el-col>
						<el-col :span="5">
							<el-form-item prop="summary">
								<UploadImg v-model:image-url="state.form.cover" height="104px">
									<template #empty>
										<el-icon><Picture /></el-icon>
										<span>请上传封面图</span>
									</template>
								</UploadImg>
							</el-form-item>
						</el-col>
					</el-row>
					<el-row class="content">
						<el-col>
							<!-- <div style="border: 1px solid #ccc; height: 100%" v-if="state.form.isHtml">
								<Toolbar style="border-bottom: 1px solid #ccc" :editor="editorRef" :defaultConfig="state.editorConfig" :mode="state.mode" />
								<Editor
									style="height: 500px; overflow-y: hidden"
									v-model="state.form.content"
									:defaultConfig="state.editorConfig"
									:mode="state.mode"
									@onCreated="onCreated"
								/>
							</div> -->
							<TinymceEditor v-model="state.form.content" v-if="state.form.isHtml" />
							<mavon-editor v-else style="height: 100%">
								<template #right-toolbar-before>
									<el-link :underline="false" title="富文本编辑器" @click="onChangeEditor">
										<template #icon>
											<el-icon :size="14"><Switch /></el-icon>
										</template>
									</el-link>
								</template>
							</mavon-editor>
						</el-col>
					</el-row>
				</div>
				<div class="right card">
					<div class="top">
						<el-row>
							<el-col class="mb20">
								<el-form-item label="发布时间" prop="publishTime">
									<el-date-picker
										v-model="state.form.publishTime"
										type="datetime"
										format="YYYY-MM-DD HH:mm:ss"
										value-format="YYYY-MM-DD HH:mm:ss"
										class="w100"
										placeholder="请选择发布时间"
										clearable
									/>
								</el-form-item>
							</el-col>
							<el-col class="mb20">
								<el-form-item label="过期时间" prop="expiredTime">
									<el-date-picker
										v-model="state.form.expiredTime"
										type="datetime"
										format="YYYY-MM-DD HH:mm:ss"
										value-format="YYYY-MM-DD HH:mm:ss"
										class="w100"
										placeholder="请选择过期时间"
										clearable
									/>
								</el-form-item>
							</el-col>
							<el-col class="mb20">
								<el-form-item label="创作类型" prop="creationType">
									<el-select v-model="state.form.creationType" placeholder="创作类型" clearable class="w100">
										<el-option label="原创" :value="0" />
										<el-option label="转载" :value="1" />
									</el-select>
								</el-form-item>
							</el-col>
							<el-col class="mb20" v-show="state.form.creationType === 1">
								<el-form-item label="外链" label-width="68px" prop="author">
									<el-input maxlength="32" v-model="state.form.link" placeholder="请输入来源外链" clearable></el-input>
								</el-form-item>
							</el-col>
							<el-col class="mb20">
								<el-form-item label="栏目" label-width="68px" prop="categoryId">
									<el-tree-select
										v-model="state.form.categoryId"
										placeholder="请选择栏目"
										:data="state.categoryData"
										check-strictly
										:render-after-expand="false"
										class="w100"
										clearable
									/>
								</el-form-item>
							</el-col>
							<el-col class="mb20">
								<el-form-item label="标签" label-width="68px" prop="tags">
									<el-select
										multiple
										v-model="state.form.tags"
										placeholder="请选择标签"
										collapse-tags
										collapse-tags-tooltip
										:max-collapse-tags="3"
										:multiple-limit="3"
										clearable
										class="w100"
									>
										<el-option v-for="item in state.tagsData" :key="item.value" :label="item.label" :value="item.value" />
									</el-select>
								</el-form-item>
							</el-col>
							<el-col class="mb20">
								<el-form-item label="作者" label-width="68px" prop="author">
									<el-input maxlength="32" v-model="state.form.author" placeholder="请输入作者" clearable></el-input>
								</el-form-item>
							</el-col>
							<el-col class="mb20">
								<el-form-item label="排序" label-width="68px" prop="sort">
									<el-input-number v-model="state.form.sort" controls-position="right" placeholder="请输入排序" class="w100" />
								</el-form-item>
							</el-col>
							<el-col class="mb20" :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
								<el-form-item label="置顶" prop="isAllowComments">
									<el-switch
										v-model="state.form.isAllowComments"
										:active-value="true"
										:inactive-value="false"
										inline-prompt
										active-text="是"
										inactive-text="否"
									></el-switch>
								</el-form-item>
							</el-col>
							<el-col class="mb20" :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
								<el-form-item label="评论" prop="isAllowComments">
									<el-switch
										v-model="state.form.isAllowComments"
										:active-value="true"
										:inactive-value="false"
										inline-prompt
										active-text="允许"
										inactive-text="禁止"
									></el-switch>
								</el-form-item>
							</el-col>
							<el-col class="mb20">
								<el-form-item label="状态" prop="status">
									<el-switch
										v-model="state.form.status"
										:active-value="0"
										:inactive-value="1"
										inline-prompt
										active-text="启用"
										inactive-text="禁用"
									></el-switch>
								</el-form-item>
							</el-col>
						</el-row>
					</div>
					<div class="bottom">
						<el-button size="default">取 消</el-button>
						<el-button type="primary" size="default" @click="onSave">保存</el-button>
					</div>
				</div>
			</el-form>
		</div>
	</div>
</template>

<script setup lang="ts">
import { reactive, onMounted, shallowRef, onBeforeUnmount } from 'vue';
import { mavonEditor } from 'mavon-editor';
import UploadImg from '/@/components/Upload/Img.vue';
import 'mavon-editor/dist/css/index.css';
import '@wangeditor/editor/dist/css/style.css';
import { Editor, Toolbar } from '@wangeditor/editor-for-vue';
import TinymceEditor from '/@/components/tinymce/index.vue';
import type { IDomEditor, IEditorConfig, IToolbarConfig } from '@wangeditor/editor';
import type { SelectOutput, TreeSelectOutput, UpdateArticleInput } from '/@/api/models';
import CategoryApi from '/@/api/CategoryApi';
import TagsApi from '/@/api/TagsApi';
const editorRef = shallowRef<IDomEditor>();
const state = reactive({
	form: {
		status: 0,
		isAllowComments: true,
		sort: 100,
		isTop: false,
		isHtml: false,
		content: '<p>hello <strong>world</strong></p>',
	} as UpdateArticleInput,
	categoryData: [] as TreeSelectOutput[],
	tagsData: [] as SelectOutput[],
	editorConfig: {
		placeholder: '请输入内容...',
	} as IEditorConfig, // 富文本编辑器配置
	toolbarConfig: {} as IToolbarConfig, // 富文本编辑器工具栏配置
	mode: 'default', // 富文本编辑器模式
});

const onChangeEditor = () => {
	state.form.isHtml = !state.form.isHtml;
};

const onCreated = (editor: any) => {
	editorRef.value = editor;
};
// 保存
const onSave = () => {};

onMounted(async () => {
	const [c, t] = await Promise.all([CategoryApi.treeSelect(), TagsApi.select()]);
	state.categoryData = c.data ?? [];
	state.tagsData = t.data ?? [];
});

// 组件销毁时，也及时销毁编辑器
onBeforeUnmount(() => {
	const editor = editorRef.value;
	if (editor == null) return;
	editor.destroy();
});
</script>

<style lang="scss" scoped>
.card {
	height: 100%;
	.form {
		height: inherit;
		display: flex;
		flex-direction: row;
		.left {
			height: inherit;
			flex: 0.8;
			padding-right: 10px;
			display: flex;
			flex-direction: column;
			:deep(.upload-box) {
				width: 100%;
				.el-upload {
					width: inherit;
				}
			}
			.title {
				flex: 0.1;
			}
			.content {
				flex: 0.9;
			}
		}
		.right {
			height: inherit;
			flex: 0.2;
			justify-content: center;
			// .card {
			display: flex;
			flex-direction: column;
			justify-content: space-between;
			// }
			.bottom {
				text-align: center;
			}
		}
	}
}
</style>
