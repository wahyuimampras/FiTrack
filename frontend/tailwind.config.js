/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        brand: {
          50:  '#f0f7f4',
          100: '#d9ede6',
          200: '#b3dbd0',
          300: '#7dbfaf',
          400: '#4d9f8b',
          500: '#2d7a64',
          600: '#285A48',
          700: '#1f4738',
          800: '#18382c',
          900: '#122b22',
          950: '#0a1a14',
        },
        income:  { 50:'#ecfdf5', 100:'#d1fae5', 500:'#10b981', 600:'#059669', 700:'#047857' },
        expense: { 50:'#fef2f2', 100:'#fee2e2', 500:'#ef4444', 600:'#dc2626', 700:'#b91c1c' },
        workout: { 50:'#f0f9ff', 100:'#e0f2fe', 500:'#0ea5e9', 600:'#0284c7' },
        canvas:  '#f6f8f7',
        surface: '#ffffff',
        sidebar: '#0f2219',
      },
      fontFamily: {
        display: ['"Instrument Serif"', 'Georgia', 'serif'],
        sans:    ['"DM Sans"', 'system-ui', 'sans-serif'],
        mono:    ['"JetBrains Mono"', 'monospace'],
      },
      borderRadius: {
        'xl': '16px', '2xl': '20px',
      },
      boxShadow: {
        'card':  '0 2px 8px rgba(15,34,25,0.06), 0 1px 3px rgba(15,34,25,0.04)',
        'brand': '0 4px 16px rgba(40,90,72,0.22)',
      },
      backgroundImage: {
        'gradient-hero':    'linear-gradient(135deg, #285A48 0%, #1a3d31 50%, #0f2820 100%)',
        'gradient-sidebar': 'linear-gradient(180deg, #0f2219 0%, #0a1a14 100%)',
        'gradient-card':    'linear-gradient(135deg, #285A48 0%, #3d7a62 100%)',
        'gradient-income':  'linear-gradient(135deg, #059669 0%, #10b981 100%)',
        'gradient-expense': 'linear-gradient(135deg, #b91c1c 0%, #ef4444 100%)',
      },
    },
  },
  plugins: [],
};