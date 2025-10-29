// tailwind.config.js
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {},
  },
  plugins: [require("daisyui")],
  safelist: [
    "alert",
    "alert-success",
    "alert-error",
    "alert-warning",
    "alert-info",
    "btn",
    "btn-sm",
    "btn-ghost",
    "toast",
    "toast-bottom",
    "toast-end",
  ],
  daisyui: {
    themes: ["light", "dark", "cupcake"], // mantenha o dark
  },
};
