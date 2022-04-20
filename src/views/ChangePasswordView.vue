<template>
    <v-card
      elevation="1"
      class="mx-auto mt-6 pa-4"
      width="30%"
    >
      <BackBtn 
        :input="backbtn" 
        @updatedInput="(value) => {this.backbtn = value}"
      />
      <v-card-title>Change Password</v-card-title>
      <v-card-text>
        <v-text-field
            v-model="newpassword"
            :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
            :rules="[rules.required, rules.min]"
            :type="show1 ? 'text' : 'password'"
            name="input-10-1"
            label="New Password"
            hint="At least 8 characters"
            counter
            outlined
            @click:append="show1 = !show1"
        ></v-text-field>
        <v-text-field
            v-model="confirmpassword"
            :append-icon="show2 ? 'mdi-eye' : 'mdi-eye-off'"
            :rules="[rules.required, rules.min, passwordConfirm]"
            :type="show2 ? 'text' : 'password'"
            name="input-10-1"
            label="Confirm Password"
            hint="At least 8 characters"
            counter
            outlined
            @click:append="show2 = !show2"
            @input="passRule"
        ></v-text-field>
      </v-card-text>
      <v-card-actions class="pl-4 pr-4">
        <v-spacer></v-spacer>
          <v-btn
            type="submit"
            color="primary"
            :disabled="invalid"
          >
            Update Password
          </v-btn>
      </v-card-actions>
    </v-card>
</template>

<script>
  import BackBtn from '@/components/FormElements/BackBtn.vue'
  export default {
    components: {
      BackBtn
    },
    data: () => ({
      backbtn:false,
      show1: false,
      show2: false,
      invalid: true,
      newpassword: '',
      confirmpassword: '',
      successPass: false,
      rules: {
        required: value => !!value || 'Required.',
        min: v => v.length >= 8 || 'Min 8 characters',
        emailMatch: () => (`The email and password you entered don't match`),
      },
    }),
    computed: {
    passwordConfirm() {
        return () => (this.newpassword === this.confirmpassword) || 'Password must match'
    },
    passRule() {
      if (this.rules.min(this.newpassword) === true && this.passwordConfirm() === true) {
        // password ok
        this.invalid = false;
      } else {
        // password wrong
        this.invalid = true;
      }
    },
    }
  }
</script>