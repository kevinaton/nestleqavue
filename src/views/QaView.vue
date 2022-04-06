<template>
<!-- <v-breadcrumbs :items="bcrumbs"></v-breadcrumbs> -->
  <v-card>
    <v-card-title>
      QA Records
      <v-spacer></v-spacer>
      <v-text-field
        v-model="search"
        append-icon="mdi-magnify"
        label="Search"
        single-line
        hide-details
      ></v-text-field>
    </v-card-title>
    <v-data-table
      :headers="headers"
      :items="food"
      :search="search"
    ></v-data-table>
  </v-card>
</template>

<script>
  export default {
    data () {
      return {
        search: '',
        headers: [
          {
            text: 'Food Items (Nutrition 100g)',
            align: 'start',
            sortable: false,
            value: 'name',
          },
          { text: 'Energy', value: 'energy' },
          { text: 'Protein', value: 'protein' },
          { text: 'Fat', value: 'fat' },
          { text: 'Carbohydrate', value: 'carbohydrate' },
          { text: 'Sodium', value: 'sodium' },
        ],
        food: [
          {
            name: 'Muesli (Almond)',
            energy: 201,
            protein: 5.0,
            fat: 34,
            carbohydrate: 5.5,
            sodium: '1.5%'
          },
          {
            name: 'Wholegrain Rolled Oats',
            energy: 301,
            protein: 3.0,
            fat: 54,
            carbohydrate: 3.5,
            sodium: '2.5%'
          },
          {
            name: 'Almond Milk',
            energy: 130,
            protein: 6.0,
            fat: 24,
            carbohydrate: 3.9,
            sodium: '3.5%'
          },
          {
            name: 'Firm Tofu',
            energy: 101,
            protein: 2.0,
            fat: 25,
            carbohydrate: 2.1,
            sodium: '0.5%'
          },
          {
            name: 'Hummus',
            energy: 709,
            protein: 8.4,
            fat: 10.8,
            carbohydrate: 4.6,
            sodium: '2.5%'
          },
          {
            name: 'Peanut Butter',
            energy: 2580,
            protein: 28,
            fat: 50,
            carbohydrate: 12,
            sodium: '3.5%'
          },
          {
            name: 'Tahini',
            energy: 2760,
            protein: 25.0,
            fat: 57.3,
            carbohydrate: 12,
            sodium: '7.0%'
          },
          {
            name: 'Butter Beans',
            energy: 384,
            protein: 7.4,
            fat: 0.9,
            carbohydrate: 15.2,
            sodium: '3.1%'
          },
          {
            name: 'Chickpeas',
            energy: 391,
            protein: 5.0,
            fat: 34,
            carbohydrate: 5.5,
            sodium: '2.3%'
          },
          {
            name: 'Lentils',
            energy: 290,
            protein: 4.2,
            fat: 0.9,
            carbohydrate: 15,
            sodium: '1.0%'
          }
        ],
      }
    },
  }
</script>
