<template>
	<div class="article-operate-container layout-padding">
		<div class="card">
			<el-form class="form">
				<div class="left">
					<el-row :gutter="20" class="title">
						<el-col :span="18">
							<el-row>
								<el-col class="mb20">
									<el-form-item label="文章标题" prop="code">
										<el-input maxlength="128" v-model="state.form.title" placeholder="请输入文章标题" clearable></el-input>
									</el-form-item>
								</el-col>
								<el-col class="mb20">
									<el-form-item label="内容摘要" prop="code">
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
						<el-col :span="6">
							<el-upload class="avatar-uploader" :on-success="onCoverSuccess" action="/api/file/upload" accept="image/*" :show-file-list="false">
								<img v-if="state.form.cover" :src="state.form.cover" class="avatar" />
								<i v-else class="avatar-uploader-icon">上传封面 </i>
							</el-upload>
						</el-col>
					</el-row>
					<el-row class="content">
						<el-col>
							<mavon-editor style="height: 100%" />
						</el-col>
					</el-row>
				</div>
				<div class="right card">
					<el-row>
						<el-col class="mb20">
							<el-form-item label="栏目" prop="categoryId">
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
							<el-form-item label="标签" prop="tags">
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
							<el-form-item label="创作类型" prop="creationType">
								<el-select v-model="state.form.creationType" placeholder="创作类型" clearable class="w100">
									<el-option label="原创" :value="0" />
									<el-option label="转载" :value="1" />
								</el-select>
							</el-form-item>
						</el-col>
					</el-row>
				</div>
			</el-form>
		</div>
	</div>
</template>

<script setup lang="ts">
import { reactive, onMounted } from 'vue';
import { mavonEditor } from 'mavon-editor';
import 'mavon-editor/dist/css/index.css';
import type { SelectOutput, TreeSelectOutput, UpdateArticleInput } from '/@/api/models';
import CategoryApi from '/@/api/CategoryApi';
import TagsApi from '/@/api/TagsApi';
const state = reactive({
	form: {} as UpdateArticleInput,
	categoryData: [] as TreeSelectOutput[],
	tagsData: [] as SelectOutput[],
});

const onCoverSuccess = (response: any) => {
	state.form.cover = response[0].url;
};

onMounted(async () => {
	const [c, t] = await Promise.all([CategoryApi.treeSelect(), TagsApi.select()]);
	//const { data } = await CategoryApi.treeSelect();
	state.categoryData = c.data ?? [];
	state.tagsData = t.data ?? [];
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
			.title {
				flex: 0.1;
			}
			.content {
				flex: 0.9;
			}
			.avatar-uploader {
				height: 104px;
				width: 100%;
				.avatar {
					width: 100%;
					height: 104px;
					display: block;
					object-fit: cover;
				}
				:deep(.el-upload) {
					border: 1px dashed var(--el-border-color);
					border-radius: 6px;
					cursor: pointer;
					position: relative;
					overflow: hidden;
					height: inherit;
					width: inherit;
					transition: var(--el-transition-duration-fast);
					:hover {
						border-color: var(--el-color-primary);
					}
					.avatar-uploader-icon {
						color: #8c939d;
						text-align: center;
					}
				}
			}
		}
		.right {
			height: inherit;
			flex: 0.2;
			justify-content: center;
		}
	}
}
</style>
