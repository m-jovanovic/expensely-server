module.exports = {
  purge: {
    enabled: true,
    content: ['./src/**/*.html', './src/**/*.ts']
  },
  darkMode: false,
  theme: {
    extend: {
      spacing: {
        88: '22rem',
        102: '26rem',
        110: '28rem'
      }
    }
  },
  variants: {
    extend: {}
  },
  plugins: []
};
