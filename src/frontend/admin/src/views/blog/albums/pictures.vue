<template>
	<div class="picture-container layout-pd">
		<el-card>
			<div class="flex-warp" v-if="state.tableData.data.length > 0">
				<el-row>
					<el-col :span="24">
						<el-upload
							accept="image/*"
							multiple
							:show-file-list="false"
							:with-credentials="true"
							action="/api/file/upload"
							:on-success="onUploadSuccess"
							><el-button type="primary">上传图片</el-button></el-upload
						>
					</el-col>
				</el-row>
				<el-row :gutter="15">
					<el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb15" v-for="(v, k) in state.tableData.data" :key="k">
						<div class="flex-warp-item">
							<div class="flex-warp-item-box">
								<div class="item-img" v-loading="v.loading">
									<img :data-img="v.img" :data-key="k" :data-lazy-img-list="k" />
									<div class="operate">
										<div
											class="handle-icon"
											@click="
												() => {
													imgViewVisible = true;
													imageUrl = v.img;
												}
											"
										>
											<el-icon><ZoomIn /></el-icon>
											<span>查看</span>
										</div>
										<div class="handle-icon" @click="onDeleteImg">
											<el-icon><Delete /></el-icon>
											<span>删除</span>
										</div>
									</div>
								</div>
							</div>
						</div>
					</el-col>
				</el-row>
			</div>
			<el-empty v-else description="暂无数据"></el-empty>
			<template v-if="state.tableData.data.length > 0">
				<el-pagination
					style="text-align: right"
					background
					@size-change="onHandleSizeChange"
					@current-change="onHandleCurrentChange"
					:page-sizes="[10, 20, 30]"
					:current-page="state.tableData.param.pageNum"
					:page-size="state.tableData.param.pageSize"
					layout="total, sizes, prev, pager, next, jumper"
					:total="state.tableData.total"
				>
				</el-pagination>
			</template>
		</el-card>
		<el-image-viewer v-if="imgViewVisible" @close="imgViewVisible = false" :url-list="[imageUrl]" />
	</div>
</template>

<script setup lang="ts" name="pagesLazyImg">
import { reactive, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import other from '/@/utils/other';
import { filterList } from '/@/views/pages/lazyImg/mock';
// 查看图片
const imgViewVisible = ref(false);
const imageUrl = ref('');
// 定义变量内容
const router = useRouter();
const state = reactive({
	tableData: {
		data: filterList,
		total: 99,
		loading: false,
		param: {
			pageNum: 1,
			pageSize: 10,
		},
	},
});

// 分页点击
const onHandleSizeChange = (val: number) => {
	state.tableData.param.pageSize = val;
};
// 分页点击
const onHandleCurrentChange = (val: number) => {
	state.tableData.param.pageNum = val;
};
const onUploadSuccess = (res: any) => {
	console.log(res);
};
const onDeleteImg = () => {};
// 页面加载时
onMounted(() => {
	other.lazyImg('[data-lazy-img-list]', state.tableData.data);
});
</script>

<style scoped lang="scss">
.picture-container {
	.flex-warp {
		display: flex;
		flex-wrap: wrap;
		align-content: flex-start;
		margin: 0 -5px;
		.flex-warp-item {
			padding: 5px;
			width: 100%;
			// height: 360px;
			.flex-warp-item-box {
				border: 1px solid var(--next-border-color-light);
				width: 100%;
				height: 100%;
				border-radius: 2px;
				display: flex;
				flex-direction: column;
				transition: all 0.3s ease;

				position: relative;
				&:hover {
					cursor: pointer;
					border: 1px solid var(--el-color-primary);
					transition: all 0.3s ease;
					box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.03);
					opacity: 1;
					.item-txt-title {
						color: var(--el-color-primary) !important;
						transition: all 0.3s ease;
					}
					.item-img {
						img {
							transition: all 0.3s ease;
							transform: translateZ(0) scale(1.05);
						}
						.operate {
							opacity: 1;
						}
					}
				}
				.item-img {
					width: 100%;
					height: 215px;
					overflow: hidden;
					img {
						transition: all 0.3s ease;
						width: 100%;
						height: 100%;
					}
					.operate {
						position: absolute;
						top: 0;
						right: 0;
						box-sizing: border-box;
						display: flex;
						align-items: center;
						justify-content: center;
						width: 100%;
						height: 100%;
						cursor: pointer;
						background: rgb(0 0 0 / 60%);
						opacity: 0;
						transition: var(--el-transition-duration-fast);
						.handle-icon {
							display: flex;
							flex-direction: column;
							align-items: center;
							justify-content: center;
							padding: 0 6%;
							color: aliceblue;
							.el-icon {
								margin-bottom: 40%;
								font-size: 130%;
								line-height: 130%;
							}
							span {
								font-size: 85%;
								line-height: 85%;
							}
						}
					}
				}
			}
		}
	}
}
</style>
