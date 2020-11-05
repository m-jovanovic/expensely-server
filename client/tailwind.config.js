module.exports = {
  future: {
    removeDeprecatedGapUtilities: true,
    purgeLayersByDefault: true,
    standardFontWeights: true
  },
  purge: ['./src/**/*.html', './src/**/*.ts'],
  theme: {
    extend: {
      spacing: {
        72: '18rem',
        80: '20rem',
        88: '22rem',
        96: '24rem',
        102: '26rem'
      }
    }
  },
  variants: {},
  plugins: []
};
