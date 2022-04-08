<template>
    <v-card
      elevation="1"
      class="mx-auto mt-6"
      width="30%"
    >
      <validation-observer
        ref="observer"
        v-slot="{}"
      >
        <form @submit.prevent="submit" class="pa-8">
          <h3 class="mb-6">Change Password</h3>
          
          <validation-provider
            v-slot="{ errors }"
            name="new_password"
            rules="required|min:8"
          >
            <v-text-field
              v-model="passsword"
              class="mb-3"
              :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
              :type="show1 ? 'text' : 'password'"
              :error-messages="errors"
              label="New Password"
              hint="At least 8 characters"
              @click:append="show1 = !show1"
              required
            ></v-text-field>
          </validation-provider>

          <validation-provider
            v-slot="{ errors }"
            name="confirm_password"
            rules="required|min:8"
          >
            <v-text-field
              v-model="password"
              class="mb-3"
              :type="show2 ? 'text' : 'password'"
              :append-icon="show2 ? 'mdi-eye' : 'mdi-eye-off'"
              :error-messages="errors"
              label="Confirm Password"
              required
              @click:append="show2 = !show2"
            ></v-text-field>
          </validation-provider>
        </form>
      </validation-observer>
      <v-card-actions>
        <v-spacer></v-spacer>
         <v-btn
            class="mr-4 mb-6"
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
  import { required, digits, email, max, min, regex } from 'vee-validate/dist/rules'
  import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from 'vee-validate'

  setInteractionMode('eager')

  extend('digits', {
    ...digits,
    message: '{_field_} needs to be {length} digits. ({_value_})',
  })

  extend('required', {
    ...required,
    message: '{_field_} can not be empty',
  })

  extend('min', {
    ...min,
    message: '{_field_} must greater than {length} characters',
  })

  extend('regex', {
    ...regex,
    message: '{_field_} {_value_} does not match {regex}',
  })

  extend('email', {
    ...email,
    message: 'Email must be valid',
  })

  export default {
    components: {
      ValidationProvider,
      ValidationObserver,
    },
    data: () => ({
      name: '',
      phoneNumber: '',
      email: '',
      show1: false,
      show2: false,
      select: null,
      items: [
        'Item 1',
        'Item 2',
        'Item 3',
        'Item 4',
      ],
    }),

    methods: {
      submit () {
        this.$refs.observer.validate()
      },
    },
  }
</script>