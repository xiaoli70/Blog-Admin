<template>
    <!-- 搜索框 -->
    <v-dialog v-bind:model-value="isShow" @update:model-value="handlerUpdateValue" max-width="600"
        :fullscreen="isMobile" scroll-strategy="none">
        <v-card class="search-wrapper" style="border-radius: 4px;text-align: center;" v-if="vm.hidesign">

            <div class="text-subtitle-1 text-medium-emphasis"></div>
            <h3 class="text-h6 mb-4">LOGIN</h3>

            <!-- <v-text-field density="compact" placeholder="Email address" prepend-inner-icon="mdi-email-outline"
                variant="outlined"></v-text-field> -->
            <v-text-field label="Email address" placeholder="johndoe@gmail.com" type="email"
                v-model="state.ruleForm.account"></v-text-field>


            <div class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between">


                <a class="text-caption text-decoration-none text-blue" href="#" rel="noopener noreferrer"
                    target="_blank">
                    Forgot login password?</a>
            </div>

            <v-text-field :append-inner-icon="vm.visible ? 'mdi-eye-off' : 'mdi-eye'"
                :type="vm.visible ? 'text' : 'password'" label="Password" placeholder="Enter your password"
                @click:append-inner="vm.visible = !vm.visible" v-model="state.ruleForm.password"></v-text-field>

            <!-- <v-card class="mb-12" color="surface-variant" variant="tonal">
                <v-card-text class="text-medium-emphasis text-caption">
                    Warning: After 3 consecutive failed login attempts, you account will be temporarily locked for three
                    hours. If you must login now, you can also click "Forgot login password?" below to reset the login
                    password.
                </v-card-text>
            </v-card> -->
            <div style="display: grid; grid-template-columns: 1fr auto;">
                <v-text-field label="Code" placeholder="验证码" type="code" v-model="state.ruleForm.code"></v-text-field>
                <el-button class="login-content-code" v-waves @click="onCaptchaChange">
                    <img :src="captchaUrl" alt="看不清？点击换一张！" />
                </el-button>
            </div>
            <v-btn class="mb-8" @click="Login" color="blue" size="large" variant="tonal" block>
                Log In
            </v-btn>

            <v-card-text class="text-center">
                <a class="text-blue text-decoration-none" href="#" @click.prevent="vm.hidesign = !vm.hidesign"
                    rel="noopener noreferrer" target="_blank">
                    Sign up now <v-icon icon="mdi-chevron-right"></v-icon>
                </a>
            </v-card-text>

        </v-card>


        <v-card class="search-wrapper" style="border-radius: 4px;text-align: center;" v-if="!vm.hidesign">
            <div class="text-subtitle-1 text-medium-emphasis"> </div>
            <h3 class="text-h6 mb-4">Verify Your Account</h3>

            <!-- <div class="text-subtitle-1 text-medium-emphasis"> Sign</div> -->

            <!-- <v-text-field density="compact" placeholder="Email address" prepend-inner-icon="mdi-email-outline"
    variant="outlined"></v-text-field> -->
            <v-text-field v-model="vm.email" label="Email address" placeholder="johndoe@gmail.com"
                type="email"></v-text-field>


            <!-- <div class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between">


                <a class="text-caption text-decoration-none text-blue" href="#" rel="noopener noreferrer"
                    target="_blank">
                    Forgot login password?</a>
            </div> -->

            <v-text-field :append-inner-icon="vm.visible ? 'mdi-eye-off' : 'mdi-eye'"
                :type="vm.visible ? 'text' : 'password'" label="Password" placeholder="Enter your password"
                @click:append-inner="vm.visible = !vm.visible" v-model="vm.password"></v-text-field>



            <v-text-field v-model="vm.code" label="Code" v-if="vm.codeinput"></v-text-field>

            <v-btn class="mb-8" color="blue" @click="GetSign" size="large" variant="tonal" block>
                Sign
            </v-btn>

            <v-card-text class="text-center">
                <a class="text-blue text-decoration-none" href="#" @click.prevent="vm.hidesign = !vm.hidesign"
                    rel="noopener noreferrer" target="_blank">
                    LogIn up now <v-icon icon="mdi-chevron-right"></v-icon>
                </a>
            </v-card-text>

        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { computed, ref, watch, watchEffect, reactive } from "vue";
import { useRouter } from "vue-router";
import dayjs from 'dayjs';
import { ArticleOutput } from "@/api/models";
import OAuthApi from "@/api/OAuthApi";
import axios from "axios";
import { useToast } from "@/stores/toast";
const props = defineProps<{
    isShow: boolean;
}>();
const toast = useToast();
const router = useRouter();
const keywords = ref<string>("");
const otp = ref("12");
const vm = reactive({
    visible: false,
    hidesign: true,
    codeinput: false,

    password: "",
    email: "",
    code: "",
});
const state = reactive({
    isShowPassword: false,
    random: new Date().getTime(),
    ruleForm: {
        account: '',
        password: '',
        code: '',
        id: dayjs().valueOf().toString(),
        referer: '',
    },
    loading: {
        signIn: false,
    },
});

const onCaptchaChange = () => {
    state.random = new Date().getTime();
};
const captchaUrl = computed(() => {
    return `http://192.168.1.101:8088/api/auth/captcha?id=${state.ruleForm.id}&r=${state.random}`;
});

const refKeywordsInput = ref<any>(null);
const emit = defineEmits<{ (e: "update:isShow", isShow: boolean): void }>();


const GetSign = async () => {
    if (vm.code == "") {
        const { statusCode, succeeded, timestamp } = await OAuthApi.SendEmail(vm.email);
        if (statusCode == 200) {
            vm.codeinput = true;
            toast.info("验证码以发至邮箱，请填写验证码.");
        }
    } else {
        var objda = {
            "email": vm.email,
            "code": vm.code,
            "password": vm.password
        };

        const { statusCode } = await OAuthApi.Register(objda);
        if (statusCode == 200) {
            alert("注册成功！");
        }
    }

};

const Login = async () => { 
    
    state.ruleForm.referer=location.href;
     const response=await OAuthApi.loginemail(state.ruleForm);
     console.log(response);
     if(response.statusCode==200){
        console.log(response.data)
        window.location.href=response.data;
     }
      

};

const handlerUpdateValue = (v: boolean) => {
    emit("update:isShow", v);
};
const articleList = reactive({
    list: [] as ArticleOutput[],
});

const isMobile = computed(() => {
    const clientWidth = document.documentElement.clientWidth;
    if (clientWidth > 960) {
        return false;
    }
    return true;
});

watch(isMobile, () => {
    emit("update:isShow", false);
});
watchEffect(() => {
    if (props.isShow && refKeywordsInput.value) {
        refKeywordsInput.value.focus();
    }
});
</script>
<style scoped>
.search-wrapper {
    padding: 1.25rem;
    height: 100%;
    background: #fff !important;
}

.search-title {
    color: #49b1f5;
    font-size: 1.25rem;
    line-height: 1;
}

.search-input-wrapper {
    display: flex;
    padding: 5px;
    height: 35px;
    width: 100%;
    border: 2px solid #8e8cd8;
    border-radius: 2rem;
}

.search-input-wrapper input {
    width: 100%;
    margin-left: 5px;
    outline: none;
}

@media (min-width: 960px) {
    .search-result-wrapper {
        padding-right: 5px;
        height: 450px;
        overflow: auto;
    }
}

@media (max-width: 959px) {
    .search-result-wrapper {
        height: calc(100vh - 110px);
        overflow: auto;
    }
}

.search-reslut a {
    color: #555;
    font-weight: bold;
    border-bottom: 1px solid #999;
    text-decoration: none;
}

.search-reslut-content {
    color: #555;
    cursor: pointer;
    border-bottom: 1px dashed #ccc;
    padding: 5px 0;
    line-height: 2;
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
}

.divider {
    margin: 20px 0;
    border: 2px dashed #d2ebfd;
}
</style>