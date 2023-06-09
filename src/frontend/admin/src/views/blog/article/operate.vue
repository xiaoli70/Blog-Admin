<template>
	<div class="article-operate-container layout-padding">
		<div class="card">
			<el-form class="form">
				<div class="left">
					<el-row :gutter="20" class="title">
						<el-col :span="18">
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
						<el-col :span="6">
							<el-form-item prop="summary">
								<UploadImg v-model:image-url="state.form.cover" height="104px">
									<template #empty>
										<el-icon><Picture /></el-icon>
										<span>请上传封面图</span>
									</template>
								</UploadImg>
							</el-form-item>
							<!-- <el-upload class="avatar-uploader" :on-success="onCoverSuccess" action="/api/file/upload" accept="image/*" :show-file-list="false">
								<img v-if="state.form.cover" :src="state.form.cover" class="avatar" />
								<i v-else class="avatar-uploader-icon">上传封面 </i>
							</el-upload> -->
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
							<el-form-item label="外链" prop="author">
								<el-input maxlength="32" v-model="state.form.link" placeholder="请输入来源外链" clearable></el-input>
							</el-form-item>
						</el-col>
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
							<el-form-item label="作者" prop="author">
								<el-input maxlength="32" v-model="state.form.author" placeholder="请输入作者" clearable></el-input>
							</el-form-item>
						</el-col>
						<el-col class="mb20">
							<el-form-item label="排序" prop="sort">
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
					<el-row>
						<el-col>
							<el-button size="default">取 消</el-button>
							<el-button type="primary" size="default">保存</el-button>
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
import UploadImg from '/@/components/Upload/Img.vue';
import 'mavon-editor/dist/css/index.css';
import type { SelectOutput, TreeSelectOutput, UpdateArticleInput } from '/@/api/models';
import CategoryApi from '/@/api/CategoryApi';
import TagsApi from '/@/api/TagsApi';
const state = reactive({
	form: {
		status: 0,
		isAllowComments: true,
		sort: 100,
		isTop: false,
	} as UpdateArticleInput,
	categoryData: [] as TreeSelectOutput[],
	tagsData: [] as SelectOutput[],
});

onMounted(async () => {
	const [c, t] = await Promise.all([CategoryApi.treeSelect(), TagsApi.select()]);
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
