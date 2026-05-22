# 🌿 FiTrack — Frontend Styling Guide
### Panduan Desain & Implementasi UI

> **Stack:** Angular 18 · Tailwind CSS · NG-Zorro · TypeScript
> **Design Language:** *Forest Refined* — Luxury-minimal dengan aksen earthy green
> **Primary Brand:** `#285A48`

---

## 📋 Daftar Isi

1. [Filosofi Desain](#1-filosofi-desain)
2. [Design Tokens](#2-design-tokens)
3. [Tipografi](#3-tipografi)
4. [Layout System](#4-layout-system)
5. [Komponen UI](#5-komponen-ui)
6. [Halaman per Fitur](#6-halaman-per-fitur)
7. [Animasi & Micro-interactions](#7-animasi--micro-interactions)
8. [Tailwind Config Lengkap](#8-tailwind-config-lengkap)
9. [NG-Zorro Theme Override](#9-ng-zorro-theme-override)
10. [Panduan Gemini AI Prompt](#10-panduan-gemini-ai-prompt)

---

## 1. Filosofi Desain

### Konsep: *"Forest Refined"*

FiTrack menggabungkan dua dunia yang jarang bertemu: **ketenangan keuangan** dan **semangat atletik**. Desainnya harus terasa seperti membuka aplikasi perbankan premium sekaligus tracker olahraga kelas atas — bukan seperti spreadsheet atau app gym generik.

**Tiga pilar desain:**

**Clarity** — Data keuangan dan workout harus terbaca sekilas. Tidak ada elemen dekoratif yang mengganggu hierarki informasi. Setiap pixel harus punya tujuan.

**Depth** — Sidebar gelap dengan konten area terang menciptakan kontras alami yang memandu mata. Layer warna dibangun berlapis seperti kanopi hutan — gelap di bawah, terang di atas.

**Warmth** — Meskipun minimalis, desain tidak boleh dingin. Warna hijau forest, font humanis, dan radius yang sedikit membulat memberikan rasa organik dan hidup.

### Referensi Visual
Inspired by: Sequence.io (layout), Linear.app (sidebar density), Loom (card elegance), Strava (data visualization boldness).

---

## 2. Design Tokens

### 2.1 Palet Warna Lengkap

```css
/* ============================================
   FITRACK DESIGN TOKENS
   Copy ke: src/styles/tokens.css
   ============================================ */

:root {

  /* ── BRAND COLORS ─────────────────────────── */
  --brand-50:   #f0f7f4;
  --brand-100:  #d9ede6;
  --brand-200:  #b3dbd0;
  --brand-300:  #7dbfaf;
  --brand-400:  #4d9f8b;
  --brand-500:  #2d7a64;
  --brand-600:  #285A48;   /* ← PRIMARY */
  --brand-700:  #1f4738;
  --brand-800:  #18382c;
  --brand-900:  #122b22;
  --brand-950:  #0a1a14;

  /* ── SIDEBAR / NAVIGATION ─────────────────── */
  --sidebar-bg:           #0f2219;  /* Lebih gelap dari brand-900 */
  --sidebar-hover:        #1a3529;
  --sidebar-active:       #285A48;
  --sidebar-active-glow:  rgba(40, 90, 72, 0.35);
  --sidebar-text:         #7fa898;
  --sidebar-text-active:  #ffffff;
  --sidebar-text-muted:   #4a6b5e;
  --sidebar-border:       rgba(255,255,255,0.05);
  --sidebar-label:        #3d6055;

  /* ── BACKGROUND ───────────────────────────── */
  --bg-canvas:   #f6f8f7;   /* Latar halaman utama — warm off-white */
  --bg-surface:  #ffffff;   /* Card, modal, dropdown */
  --bg-elevated: #ffffff;   /* Panel yang "naik" dari canvas */
  --bg-muted:    #f0f4f2;   /* Input disabled, skeleton */
  --bg-overlay:  rgba(15, 34, 25, 0.55);  /* Modal backdrop */

  /* ── TEXT ─────────────────────────────────── */
  --text-primary:   #0f2219;  /* Heading utama */
  --text-secondary: #3d5a50;  /* Body text, label */
  --text-muted:     #7a9d91;  /* Placeholder, caption */
  --text-inverse:   #ffffff;  /* Teks di atas bg gelap */
  --text-brand:     #285A48;  /* Link, highlight text */
  --text-link:      #1f7a5e;

  /* ── BORDER ───────────────────────────────── */
  --border-subtle:  rgba(40, 90, 72, 0.08);
  --border-default: rgba(40, 90, 72, 0.15);
  --border-strong:  rgba(40, 90, 72, 0.3);
  --border-brand:   #285A48;

  /* ── SEMANTIC: SUCCESS ────────────────────── */
  --success-50:   #f0f7f4;
  --success-100:  #d9ede6;
  --success-500:  #285A48;
  --success-600:  #1f4738;
  --success-text: #1f4738;
  --success-bg:   #d9ede6;

  /* ── SEMANTIC: INCOME (hijau positif) ───────── */
  --income-50:   #ecfdf5;
  --income-100:  #d1fae5;
  --income-500:  #10b981;
  --income-600:  #059669;
  --income-700:  #047857;
  --income-bg:   #d1fae5;
  --income-text: #065f46;

  /* ── SEMANTIC: EXPENSE (merah negatif) ────── */
  --expense-50:   #fef2f2;
  --expense-100:  #fee2e2;
  --expense-500:  #ef4444;
  --expense-600:  #dc2626;
  --expense-700:  #b91c1c;
  --expense-bg:   #fee2e2;
  --expense-text: #991b1b;

  /* ── SEMANTIC: WARNING ────────────────────── */
  --warning-50:   #fffbeb;
  --warning-100:  #fef3c7;
  --warning-500:  #f59e0b;
  --warning-600:  #d97706;
  --warning-bg:   #fef3c7;
  --warning-text: #92400e;

  /* ── SEMANTIC: INFO ───────────────────────── */
  --info-50:   #eff6ff;
  --info-100:  #dbeafe;
  --info-500:  #3b82f6;
  --info-600:  #2563eb;
  --info-bg:   #dbeafe;
  --info-text: #1e40af;

  /* ── WORKOUT ACCENT (biru-teal untuk data Strava) */
  --strava-orange:   #FC4C02;   /* Warna resmi Strava — untuk badge koneksi */
  --workout-accent:  #0ea5e9;   /* Warna aktivitas, grafik workout */
  --workout-50:      #f0f9ff;
  --workout-100:     #e0f2fe;
  --workout-600:     #0284c7;

  /* ── GRADIENTS ────────────────────────────── */
  --gradient-sidebar:  linear-gradient(180deg, #0f2219 0%, #0a1a14 100%);
  --gradient-hero:     linear-gradient(135deg, #285A48 0%, #1a3d31 50%, #0f2820 100%);
  --gradient-card-primary: linear-gradient(135deg, #285A48 0%, #3d7a62 100%);
  --gradient-income:   linear-gradient(135deg, #059669 0%, #10b981 100%);
  --gradient-expense:  linear-gradient(135deg, #b91c1c 0%, #ef4444 100%);
  --gradient-workout:  linear-gradient(135deg, #0284c7 0%, #0ea5e9 100%);
  --gradient-surface:  linear-gradient(180deg, #ffffff 0%, #f6f8f7 100%);

  /* ── SHADOWS ──────────────────────────────── */
  --shadow-xs:  0 1px 2px rgba(15, 34, 25, 0.05);
  --shadow-sm:  0 1px 4px rgba(15, 34, 25, 0.07), 0 1px 2px rgba(15, 34, 25, 0.05);
  --shadow-md:  0 4px 12px rgba(15, 34, 25, 0.08), 0 2px 6px rgba(15, 34, 25, 0.05);
  --shadow-lg:  0 8px 24px rgba(15, 34, 25, 0.10), 0 4px 12px rgba(15, 34, 25, 0.06);
  --shadow-xl:  0 16px 40px rgba(15, 34, 25, 0.12), 0 8px 20px rgba(15, 34, 25, 0.07);
  --shadow-brand: 0 4px 16px rgba(40, 90, 72, 0.22);
  --shadow-card:  0 2px 8px rgba(15, 34, 25, 0.06), 0 1px 3px rgba(15, 34, 25, 0.04);

  /* ── SPACING SCALE ────────────────────────── */
  /* Mengikuti 4px base grid (Golden Rule: 4-8-12-16-20-24-32-40-48-64-80) */
  --space-1:  4px;
  --space-2:  8px;
  --space-3:  12px;
  --space-4:  16px;
  --space-5:  20px;
  --space-6:  24px;
  --space-8:  32px;
  --space-10: 40px;
  --space-12: 48px;
  --space-16: 64px;
  --space-20: 80px;

  /* ── BORDER RADIUS ────────────────────────── */
  --radius-xs: 4px;    /* Badge, tag kecil */
  --radius-sm: 6px;    /* Button kecil, input */
  --radius-md: 8px;    /* Button default, tooltip */
  --radius-lg: 12px;   /* Card kecil, dropdown */
  --radius-xl: 16px;   /* Card utama */
  --radius-2xl: 20px;  /* Hero card, modal */
  --radius-full: 9999px; /* Pill, avatar, toggle */

  /* ── Z-INDEX ──────────────────────────────── */
  --z-dropdown: 100;
  --z-sticky:   200;
  --z-sidebar:  300;
  --z-modal:    400;
  --z-toast:    500;
  --z-tooltip:  600;

  /* ── TRANSITIONS ──────────────────────────── */
  --transition-fast:   80ms cubic-bezier(0.4, 0, 0.2, 1);
  --transition-base:   150ms cubic-bezier(0.4, 0, 0.2, 1);
  --transition-slow:   250ms cubic-bezier(0.4, 0, 0.2, 1);
  --transition-spring: 350ms cubic-bezier(0.34, 1.56, 0.64, 1);

  /* ── LAYOUT ───────────────────────────────── */
  --sidebar-width:         240px;
  --sidebar-collapsed-width: 64px;
  --topbar-height:         64px;
  --content-max-width:     1280px;
  --content-padding:       24px;
}
```

### 2.2 Tabel Token Cepat (Referensi Coding)

| Token | Hex | Digunakan Untuk |
|-------|-----|-----------------|
| `--brand-600` | `#285A48` | Primary button, active nav, border accent |
| `--brand-700` | `#1f4738` | Primary button hover |
| `--brand-900` | `#122b22` | Sidebar text pada bg gelap |
| `--sidebar-bg` | `#0f2219` | Background sidebar |
| `--bg-canvas` | `#f6f8f7` | Background halaman |
| `--bg-surface` | `#ffffff` | Card & panel |
| `--text-primary` | `#0f2219` | Heading, teks penting |
| `--text-secondary` | `#3d5a50` | Body text, label |
| `--text-muted` | `#7a9d91` | Caption, placeholder |
| `--income-600` | `#059669` | Angka pemasukan, trend positif |
| `--expense-600` | `#dc2626` | Angka pengeluaran, trend negatif |
| `--workout-accent` | `#0ea5e9` | Data workout, grafik aktivitas |
| `--strava-orange` | `#FC4C02` | Badge Strava, tombol connect |

---

## 3. Tipografi

### 3.1 Font Stack

```css
/* Import di index.html atau styles.css */
@import url('https://fonts.googleapis.com/css2?family=Instrument+Serif:ital@0;1&family=DM+Sans:ital,opsz,wght@0,9..40,300;0,9..40,400;0,9..40,500;0,9..40,600;1,9..40,300&display=swap');

/* Font Stack */
--font-display: 'Instrument Serif', Georgia, serif;
  /* Untuk: angka besar di dashboard, hero headline */

--font-body: 'DM Sans', -apple-system, BlinkMacSystemFont, sans-serif;
  /* Untuk: semua UI text, label, body copy */

--font-mono: 'JetBrains Mono', 'Fira Code', 'Cascadia Code', monospace;
  /* Untuk: nilai numerik akun, kode transaksi, ID */
```

**Alasan pemilihan font:**
- **Instrument Serif** — Font serif humanis yang modern dan elegan. Memberikan karakter unik pada angka-angka besar (saldo, total jarak) tanpa terasa kuno. Kontras sempurna dengan UI sans-serif.
- **DM Sans** — Sans-serif geometris dengan optical sizing. Terbaca sangat baik di ukuran kecil (label, badge) dan tetap elegan di ukuran besar. Karakternya sedikit lebih "warm" dibanding Inter.
- **JetBrains Mono** — Untuk kode transaksi dan nilai numerik penting — terasa presisi dan data-driven.

### 3.2 Skala Tipografi

```css
/* ── TYPE SCALE (berdasarkan 1rem = 16px) ── */

/* Display — Angka besar saldo, hero number */
.text-display-2xl { font-family: var(--font-display); font-size: 48px; font-weight: 400; line-height: 1.1; letter-spacing: -0.02em; }
.text-display-xl  { font-family: var(--font-display); font-size: 36px; font-weight: 400; line-height: 1.15; letter-spacing: -0.015em; }
.text-display-lg  { font-family: var(--font-display); font-size: 28px; font-weight: 400; line-height: 1.2; letter-spacing: -0.01em; }

/* Heading — Judul section, card title */
.text-h1  { font-family: var(--font-body); font-size: 24px; font-weight: 500; line-height: 1.3; letter-spacing: -0.008em; }
.text-h2  { font-family: var(--font-body); font-size: 20px; font-weight: 500; line-height: 1.35; letter-spacing: -0.005em; }
.text-h3  { font-family: var(--font-body); font-size: 16px; font-weight: 500; line-height: 1.4; }

/* Body — Teks konten */
.text-body-lg { font-family: var(--font-body); font-size: 16px; font-weight: 400; line-height: 1.6; }
.text-body    { font-family: var(--font-body); font-size: 14px; font-weight: 400; line-height: 1.6; }
.text-body-sm { font-family: var(--font-body); font-size: 13px; font-weight: 400; line-height: 1.5; }

/* Label — Form label, nav item, table header */
.text-label    { font-family: var(--font-body); font-size: 13px; font-weight: 500; line-height: 1.4; letter-spacing: 0.005em; }
.text-label-sm { font-family: var(--font-body); font-size: 11px; font-weight: 500; line-height: 1.4; letter-spacing: 0.04em; text-transform: uppercase; }

/* Caption — Timestamp, helper text */
.text-caption { font-family: var(--font-body); font-size: 12px; font-weight: 400; line-height: 1.5; color: var(--text-muted); }

/* Numeric — Angka data, nilai keuangan */
.text-numeric    { font-family: var(--font-mono); font-size: 14px; font-weight: 500; letter-spacing: -0.01em; }
.text-numeric-lg { font-family: var(--font-mono); font-size: 18px; font-weight: 500; letter-spacing: -0.015em; }
```

### 3.3 Aturan Penggunaan Tipografi

| Elemen | Font | Size | Weight | Color |
|--------|------|------|--------|-------|
| Total saldo (hero) | Instrument Serif | 48px | 400 | `--text-primary` |
| Jumlah transaksi card | Instrument Serif | 28px | 400 | Sesuai konteks |
| Page title | DM Sans | 24px | 500 | `--text-primary` |
| Section heading | DM Sans | 16px | 500 | `--text-primary` |
| Body / description | DM Sans | 14px | 400 | `--text-secondary` |
| Nav item | DM Sans | 13px | 500 | `--sidebar-text` |
| Table header | DM Sans | 11px | 500 | `--text-muted` (uppercase) |
| Badge / tag | DM Sans | 11px | 500 | Sesuai semantic |
| Nilai rekening | JetBrains Mono | 14px | 500 | `--text-primary` |
| Kode transaksi / ID | JetBrains Mono | 12px | 400 | `--text-muted` |

---

## 4. Layout System

### 4.1 Struktur Layout Utama

```
┌──────────────────────────────────────────────────────┐
│ [SIDEBAR 240px]         [MAIN CONTENT AREA]          │
│                                                      │
│  Logo                   [TOPBAR 64px height]         │
│                         Search ── Date ── User       │
│  ─────────────                                       │
│  GENERAL                [CONTENT WRAPPER]            │
│  ● Dashboard            padding: 24px                │
│    Payment              max-width: 1280px            │
│    Transaction                                       │
│    Cards                [PAGE CONTENT]               │
│                         Grid 12 kolom                │
│  ─────────────                                       │
│  SUPPORT                                             │
│    Capital                                           │
│    Vaults                                            │
│    Reports                                           │
│    Strava Sync                                       │
│                                                      │
│  ─────────────                                       │
│  [BOTTOM]                                            │
│    Settings                                          │
│    Help                                              │
│    [User Avatar]                                     │
└──────────────────────────────────────────────────────┘
```

### 4.2 Sidebar — Menu & Submenu Lengkap

```
FITRACK
├── GENERAL
│   ├── 🏠 Dashboard          /dashboard
│   ├── 💳 Accounts           /accounts
│   └── 🔄 Transactions       /transactions
│
├── FINANCE
│   ├── 📊 Budgets            /budgets
│   ├── 🎯 Saving Goals       /saving-goals
│   └── 📋 Recurring Bills    /recurring-bills
│
├── WORKOUT
│   ├── 🏃 Activities         /activities
│   ├── 📈 Stats & Records    /workout/stats
│   └── ⚡ Strava Sync        /strava           [badge: "Connected" / "Connect"]
│
├── INSIGHTS
│   └── 📉 Reports            /reports          [submenu: Monthly, Annual, Export]
│
└── [BOTTOM SECTION]
    ├── ⚙️  Settings          /settings
    ├── ❓  Help              /help
    └── [USER CARD]           nama + email
```

### 4.3 CSS Layout Implementation

```css
/* ── APP SHELL ────────────────────────────── */
.app-shell {
  display: flex;
  height: 100vh;
  overflow: hidden;
  background: var(--bg-canvas);
}

/* ── SIDEBAR ──────────────────────────────── */
.sidebar {
  width: var(--sidebar-width);
  min-height: 100vh;
  background: var(--gradient-sidebar);
  display: flex;
  flex-direction: column;
  flex-shrink: 0;
  border-right: 1px solid var(--sidebar-border);
  position: relative;
  z-index: var(--z-sidebar);
  overflow-y: auto;
  overflow-x: hidden;
  scrollbar-width: none;  /* Firefox */
}
.sidebar::-webkit-scrollbar { display: none; }  /* Chrome */

/* ── MAIN AREA ────────────────────────────── */
.main-area {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

/* ── TOPBAR ───────────────────────────────── */
.topbar {
  height: var(--topbar-height);
  background: var(--bg-surface);
  border-bottom: 1px solid var(--border-subtle);
  display: flex;
  align-items: center;
  padding: 0 var(--content-padding);
  gap: var(--space-4);
  flex-shrink: 0;
  position: sticky;
  top: 0;
  z-index: var(--z-sticky);
}

/* ── CONTENT WRAPPER ──────────────────────── */
.content-wrapper {
  flex: 1;
  overflow-y: auto;
  padding: var(--content-padding);
}

.content-inner {
  max-width: var(--content-max-width);
  margin: 0 auto;
}

/* ── GRID SYSTEM ──────────────────────────── */
.grid-dashboard {
  display: grid;
  grid-template-columns: repeat(12, 1fr);
  gap: var(--space-4);
}

/* Kolom preset yang sering dipakai */
.col-3  { grid-column: span 3; }   /* 1/4 lebar — stat card kecil */
.col-4  { grid-column: span 4; }   /* 1/3 lebar — card medium */
.col-6  { grid-column: span 6; }   /* 1/2 lebar — split panel */
.col-7  { grid-column: span 7; }   /* chart utama */
.col-8  { grid-column: span 8; }   /* konten utama */
.col-9  { grid-column: span 9; }   /* konten + sidebar kecil */
.col-12 { grid-column: span 12; }  /* full width */
```

---

## 5. Komponen UI

### 5.1 Sidebar

```css
/* ── SIDEBAR LOGO ─────────────────────────── */
.sidebar-logo {
  padding: 20px var(--space-6);
  display: flex;
  align-items: center;
  gap: var(--space-3);
  border-bottom: 1px solid var(--sidebar-border);
  margin-bottom: var(--space-2);
}

.sidebar-logo-mark {
  width: 32px;
  height: 32px;
  background: var(--brand-600);
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
  /* Isi dengan icon daun/sprout SVG */
}

.sidebar-logo-text {
  font-family: var(--font-body);
  font-size: 15px;
  font-weight: 600;
  color: var(--text-inverse);
  letter-spacing: -0.01em;
}

/* ── SECTION LABEL ────────────────────────── */
.sidebar-section-label {
  padding: var(--space-4) var(--space-6) var(--space-2);
  font-family: var(--font-body);
  font-size: 10px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: var(--sidebar-label);
}

/* ── NAV ITEM ─────────────────────────────── */
.nav-item {
  display: flex;
  align-items: center;
  gap: var(--space-3);
  padding: 9px var(--space-6);
  margin: 1px var(--space-3);
  border-radius: var(--radius-md);
  color: var(--sidebar-text);
  font-size: 13px;
  font-weight: 500;
  cursor: pointer;
  text-decoration: none;
  transition: background var(--transition-base), color var(--transition-base);
  position: relative;
}

.nav-item:hover {
  background: var(--sidebar-hover);
  color: rgba(255,255,255,0.85);
}

.nav-item.active {
  background: var(--sidebar-active);
  color: var(--sidebar-text-active);
  box-shadow: var(--shadow-brand);
}

/* Indikator active kiri */
.nav-item.active::before {
  content: '';
  position: absolute;
  left: calc(-1 * var(--space-3));
  top: 50%;
  transform: translateY(-50%);
  width: 3px;
  height: 20px;
  background: #5bcea8;
  border-radius: 0 var(--radius-xs) var(--radius-xs) 0;
}

.nav-item-icon {
  width: 18px;
  height: 18px;
  opacity: 0.7;
  flex-shrink: 0;
}

.nav-item.active .nav-item-icon { opacity: 1; }

/* ── NAV BADGE ────────────────────────────── */
.nav-badge {
  margin-left: auto;
  font-size: 10px;
  font-weight: 600;
  padding: 2px 7px;
  border-radius: var(--radius-full);
}

.nav-badge.success {
  background: rgba(16, 185, 129, 0.15);
  color: #34d399;
}

.nav-badge.warning {
  background: rgba(245, 158, 11, 0.15);
  color: #fbbf24;
}

.nav-badge.strava {
  background: rgba(252, 76, 2, 0.15);
  color: #FC4C02;
}

/* ── USER CARD (bottom sidebar) ───────────── */
.sidebar-user-card {
  margin: auto var(--space-3) var(--space-4);
  padding: var(--space-3) var(--space-4);
  background: rgba(255,255,255,0.05);
  border: 1px solid var(--sidebar-border);
  border-radius: var(--radius-lg);
  display: flex;
  align-items: center;
  gap: var(--space-3);
  cursor: pointer;
  transition: background var(--transition-base);
}

.sidebar-user-card:hover {
  background: rgba(255,255,255,0.08);
}

.sidebar-avatar {
  width: 32px;
  height: 32px;
  border-radius: var(--radius-full);
  background: var(--brand-600);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 13px;
  font-weight: 600;
  color: #ffffff;
  flex-shrink: 0;
}

.sidebar-user-name {
  font-size: 12px;
  font-weight: 500;
  color: rgba(255,255,255,0.85);
  line-height: 1.3;
}

.sidebar-user-email {
  font-size: 11px;
  color: var(--sidebar-text-muted);
  line-height: 1.3;
}
```

### 5.2 Topbar

```css
/* ── TOPBAR ───────────────────────────────── */
.topbar-search {
  display: flex;
  align-items: center;
  gap: var(--space-2);
  background: var(--bg-canvas);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-full);
  padding: var(--space-2) var(--space-4);
  min-width: 240px;
  cursor: pointer;
  transition: border-color var(--transition-base), box-shadow var(--transition-base);
}

.topbar-search:hover {
  border-color: var(--border-default);
  box-shadow: var(--shadow-sm);
}

.topbar-search-icon { color: var(--text-muted); width: 16px; height: 16px; }

.topbar-search-text {
  font-size: 13px;
  color: var(--text-muted);
  flex: 1;
}

.topbar-shortcut {
  font-size: 11px;
  font-family: var(--font-mono);
  color: var(--text-muted);
  background: var(--bg-muted);
  padding: 1px 6px;
  border-radius: var(--radius-xs);
  border: 1px solid var(--border-subtle);
}

/* ── DATE RANGE PICKER ────────────────────── */
.topbar-daterange {
  display: flex;
  align-items: center;
  gap: var(--space-2);
  padding: var(--space-2) var(--space-4);
  border: 1px solid var(--border-default);
  border-radius: var(--radius-md);
  font-size: 13px;
  font-weight: 500;
  color: var(--text-secondary);
  cursor: pointer;
  transition: all var(--transition-base);
}

.topbar-daterange:hover {
  background: var(--bg-canvas);
  border-color: var(--brand-600);
  color: var(--brand-600);
}

/* ── TOPBAR ACTIONS ───────────────────────── */
.topbar-actions {
  display: flex;
  align-items: center;
  gap: var(--space-2);
  margin-left: auto;
}
```

### 5.3 Buttons

```css
/* ── BUTTON BASE ──────────────────────────── */
.btn {
  display: inline-flex;
  align-items: center;
  gap: var(--space-2);
  font-family: var(--font-body);
  font-weight: 500;
  border-radius: var(--radius-md);
  border: 1px solid transparent;
  cursor: pointer;
  transition: all var(--transition-base);
  text-decoration: none;
  white-space: nowrap;
}

/* Size variants */
.btn-xs  { padding: 4px 10px;  font-size: 11px; border-radius: var(--radius-sm); }
.btn-sm  { padding: 6px 14px;  font-size: 13px; }
.btn-md  { padding: 9px 18px;  font-size: 14px; }
.btn-lg  { padding: 12px 24px; font-size: 15px; border-radius: var(--radius-lg); }

/* ── PRIMARY ──────────────────────────────── */
.btn-primary {
  background: var(--brand-600);
  color: #ffffff;
  border-color: var(--brand-600);
  box-shadow: var(--shadow-brand);
}
.btn-primary:hover {
  background: var(--brand-700);
  border-color: var(--brand-700);
  box-shadow: 0 6px 20px rgba(40, 90, 72, 0.30);
  transform: translateY(-1px);
}
.btn-primary:active {
  transform: translateY(0);
  box-shadow: var(--shadow-sm);
}

/* ── SECONDARY ────────────────────────────── */
.btn-secondary {
  background: var(--bg-surface);
  color: var(--text-primary);
  border-color: var(--border-default);
  box-shadow: var(--shadow-xs);
}
.btn-secondary:hover {
  background: var(--bg-canvas);
  border-color: var(--border-strong);
  box-shadow: var(--shadow-sm);
}

/* ── GHOST ────────────────────────────────── */
.btn-ghost {
  background: transparent;
  color: var(--text-secondary);
  border-color: transparent;
}
.btn-ghost:hover {
  background: var(--bg-canvas);
  color: var(--text-primary);
}

/* ── DANGER ───────────────────────────────── */
.btn-danger {
  background: var(--expense-600);
  color: #ffffff;
}
.btn-danger:hover {
  background: var(--expense-700);
  transform: translateY(-1px);
}

/* ── STRAVA (brand orange) ────────────────── */
.btn-strava {
  background: var(--strava-orange);
  color: #ffffff;
  box-shadow: 0 4px 14px rgba(252, 76, 2, 0.25);
}
.btn-strava:hover {
  background: #e04402;
  transform: translateY(-1px);
}

/* ── ICON BUTTON ──────────────────────────── */
.btn-icon {
  width: 36px;
  height: 36px;
  padding: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: var(--radius-md);
}
.btn-icon-sm { width: 28px; height: 28px; }
.btn-icon-lg { width: 44px; height: 44px; border-radius: var(--radius-lg); }
```

### 5.4 Cards

```css
/* ── BASE CARD ────────────────────────────── */
.card {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-xl);
  padding: var(--space-6);
  box-shadow: var(--shadow-card);
  transition: box-shadow var(--transition-base), transform var(--transition-slow);
}

.card:hover {
  box-shadow: var(--shadow-md);
}

/* ── HERO CARD (saldo utama di dashboard) ─── */
.card-hero {
  background: var(--gradient-hero);
  border: none;
  border-radius: var(--radius-2xl);
  padding: var(--space-8);
  color: #ffffff;
  position: relative;
  overflow: hidden;
}

/* Decorative subtle pattern on hero */
.card-hero::after {
  content: '';
  position: absolute;
  top: -40px;
  right: -40px;
  width: 200px;
  height: 200px;
  border-radius: 50%;
  background: rgba(255,255,255,0.04);
  pointer-events: none;
}

.card-hero-balance-label {
  font-size: 12px;
  font-weight: 500;
  color: rgba(255,255,255,0.6);
  text-transform: uppercase;
  letter-spacing: 0.06em;
  margin-bottom: var(--space-2);
}

.card-hero-balance-value {
  font-family: var(--font-display);
  font-size: 44px;
  font-weight: 400;
  color: #ffffff;
  letter-spacing: -0.02em;
  line-height: 1.1;
}

.card-hero-badge {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  background: rgba(255,255,255,0.12);
  color: rgba(255,255,255,0.9);
  font-size: 12px;
  font-weight: 500;
  padding: 3px 10px;
  border-radius: var(--radius-full);
  margin-left: var(--space-3);
}

/* ── STAT CARD (4 kolom di dashboard) ─────── */
.card-stat {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-xl);
  padding: var(--space-5) var(--space-6);
  box-shadow: var(--shadow-card);
  display: flex;
  flex-direction: column;
  gap: var(--space-3);
}

.card-stat-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.card-stat-icon {
  width: 36px;
  height: 36px;
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
}

.card-stat-icon.income  { background: var(--income-bg); color: var(--income-600); }
.card-stat-icon.expense { background: var(--expense-bg); color: var(--expense-600); }
.card-stat-icon.workout { background: var(--workout-100); color: var(--workout-600); }
.card-stat-icon.savings { background: var(--brand-100); color: var(--brand-600); }

.card-stat-value {
  font-family: var(--font-display);
  font-size: 26px;
  font-weight: 400;
  color: var(--text-primary);
  letter-spacing: -0.015em;
  line-height: 1.2;
}

.card-stat-label {
  font-size: 12px;
  font-weight: 500;
  color: var(--text-muted);
}

.card-stat-trend {
  display: inline-flex;
  align-items: center;
  gap: 3px;
  font-size: 12px;
  font-weight: 500;
  padding: 2px 8px;
  border-radius: var(--radius-full);
}

.card-stat-trend.up   { background: var(--income-bg); color: var(--income-text); }
.card-stat-trend.down { background: var(--expense-bg); color: var(--expense-text); }

/* ── ACCOUNT CARD ─────────────────────────── */
.card-account {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-xl);
  padding: var(--space-5);
  box-shadow: var(--shadow-card);
  cursor: pointer;
  transition: all var(--transition-base);
}

.card-account:hover {
  border-color: var(--brand-600);
  box-shadow: var(--shadow-brand);
  transform: translateY(-2px);
}

/* ── ACTIVITY CARD (workout) ──────────────── */
.card-activity {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-xl);
  padding: var(--space-5);
  box-shadow: var(--shadow-card);
  display: flex;
  gap: var(--space-4);
  align-items: flex-start;
}

.card-activity-icon {
  width: 44px;
  height: 44px;
  border-radius: var(--radius-lg);
  background: var(--workout-100);
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--workout-600);
  flex-shrink: 0;
}

/* ── BUDGET CARD ──────────────────────────── */
.card-budget {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-xl);
  padding: var(--space-5) var(--space-6);
  box-shadow: var(--shadow-card);
}

/* Progress bar di budget card */
.budget-progress-track {
  width: 100%;
  height: 6px;
  background: var(--bg-muted);
  border-radius: var(--radius-full);
  overflow: hidden;
  margin-top: var(--space-3);
}

.budget-progress-fill {
  height: 100%;
  border-radius: var(--radius-full);
  transition: width 0.6s cubic-bezier(0.34, 1.56, 0.64, 1);
}

.budget-progress-fill.safe    { background: var(--brand-600); }
.budget-progress-fill.warning { background: var(--warning-500); }
.budget-progress-fill.danger  { background: var(--expense-600); }
```

### 5.5 Form & Input

```css
/* ── INPUT BASE ───────────────────────────── */
.input {
  width: 100%;
  padding: 10px var(--space-4);
  font-family: var(--font-body);
  font-size: 14px;
  color: var(--text-primary);
  background: var(--bg-surface);
  border: 1px solid var(--border-default);
  border-radius: var(--radius-md);
  outline: none;
  transition: border-color var(--transition-fast), box-shadow var(--transition-fast);
}

.input::placeholder { color: var(--text-muted); }

.input:hover  { border-color: var(--border-strong); }

.input:focus  {
  border-color: var(--brand-600);
  box-shadow: 0 0 0 3px rgba(40, 90, 72, 0.12);
}

/* ── INPUT SIZE ───────────────────────────── */
.input-sm { padding: 6px 12px; font-size: 13px; }
.input-lg { padding: 12px var(--space-5); font-size: 15px; }

/* ── INPUT GROUP (prefix/suffix) ─────────── */
.input-group {
  display: flex;
  align-items: center;
  border: 1px solid var(--border-default);
  border-radius: var(--radius-md);
  overflow: hidden;
  transition: border-color var(--transition-fast), box-shadow var(--transition-fast);
}

.input-group:focus-within {
  border-color: var(--brand-600);
  box-shadow: 0 0 0 3px rgba(40, 90, 72, 0.12);
}

.input-prefix,
.input-suffix {
  padding: 0 var(--space-3);
  background: var(--bg-muted);
  color: var(--text-muted);
  font-size: 13px;
  font-weight: 500;
  border: none;
  height: 100%;
  display: flex;
  align-items: center;
}

.input-group .input {
  border: none;
  box-shadow: none;
  flex: 1;
  border-radius: 0;
}

/* ── LABEL ────────────────────────────────── */
.form-label {
  display: block;
  font-size: 13px;
  font-weight: 500;
  color: var(--text-secondary);
  margin-bottom: var(--space-2);
}

.form-help {
  font-size: 12px;
  color: var(--text-muted);
  margin-top: var(--space-1);
}

.form-error {
  font-size: 12px;
  color: var(--expense-600);
  margin-top: var(--space-1);
}

/* ── SELECT ───────────────────────────────── */
.select {
  /* Sama dengan .input + */
  appearance: none;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24' fill='none' stroke='%237a9d91' stroke-width='2'%3E%3Cpolyline points='6 9 12 15 18 9'%3E%3C/polyline%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 14px center;
  padding-right: 36px;
  cursor: pointer;
}
```

### 5.6 Badge & Tag

```css
/* ── BADGE ────────────────────────────────── */
.badge {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  font-size: 11px;
  font-weight: 600;
  padding: 3px 9px;
  border-radius: var(--radius-full);
  letter-spacing: 0.01em;
}

/* Variants */
.badge-success { background: var(--success-bg); color: var(--success-text); }
.badge-danger  { background: var(--expense-bg); color: var(--expense-text); }
.badge-warning { background: var(--warning-bg); color: var(--warning-text); }
.badge-info    { background: var(--info-bg);    color: var(--info-text); }
.badge-brand   { background: var(--brand-100);  color: var(--brand-700); }
.badge-workout { background: var(--workout-100); color: var(--workout-600); }
.badge-strava  { background: rgba(252,76,2,0.1); color: var(--strava-orange); }
.badge-neutral { background: var(--bg-muted);   color: var(--text-secondary); }

/* Dot indicator */
.badge::before {
  content: '';
  width: 5px;
  height: 5px;
  border-radius: 50%;
  background: currentColor;
}

/* ── TYPE TAG (untuk jenis transaksi) ─────── */
.tag-income  { background: var(--income-bg);  color: var(--income-text);  }
.tag-expense { background: var(--expense-bg); color: var(--expense-text); }
.tag-transfer { background: var(--info-bg);   color: var(--info-text);    }
```

### 5.7 Table (Transactions)

```css
/* ── TABLE ────────────────────────────────── */
.table-wrapper {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-xl);
  overflow: hidden;
  box-shadow: var(--shadow-card);
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.table-header {
  background: var(--bg-canvas);
  border-bottom: 1px solid var(--border-subtle);
}

.table-th {
  padding: var(--space-3) var(--space-5);
  font-size: 11px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  color: var(--text-muted);
  text-align: left;
}

.table-tr {
  border-bottom: 1px solid var(--border-subtle);
  transition: background var(--transition-fast);
}

.table-tr:last-child { border-bottom: none; }

.table-tr:hover {
  background: var(--bg-canvas);
}

.table-td {
  padding: var(--space-4) var(--space-5);
  font-size: 14px;
  color: var(--text-secondary);
  vertical-align: middle;
}

/* Transaction amount styling */
.amount-income  { color: var(--income-600); font-family: var(--font-mono); font-weight: 500; }
.amount-expense { color: var(--expense-600); font-family: var(--font-mono); font-weight: 500; }
```

### 5.8 Toggle / Switch

```css
/* ── TOGGLE (misal: Pro Mode di sidebar) ──── */
.toggle {
  position: relative;
  width: 40px;
  height: 22px;
  cursor: pointer;
}

.toggle input { opacity: 0; width: 0; height: 0; }

.toggle-track {
  position: absolute;
  inset: 0;
  background: var(--border-default);
  border-radius: var(--radius-full);
  transition: background var(--transition-base);
}

.toggle input:checked + .toggle-track {
  background: var(--brand-600);
}

.toggle-thumb {
  position: absolute;
  top: 3px;
  left: 3px;
  width: 16px;
  height: 16px;
  background: white;
  border-radius: 50%;
  box-shadow: var(--shadow-sm);
  transition: transform var(--transition-spring);
}

.toggle input:checked ~ .toggle-thumb {
  transform: translateX(18px);
}
```

---

## 6. Halaman per Fitur

### 6.1 Dashboard Layout

```
┌─────────────────────────────────────────────────────┐
│ TOPBAR: Search ── 18 Oct - 18 Nov 2024 ── Notif ── User │
├─────────────────────────────────────────────────────┤
│                                                     │
│  ┌──────────────── HERO CARD (col-12) ──────────┐   │
│  │  Total Balance              [+ Add] [↑ Send] │   │
│  │  Rp 48.250.000  +15.8%↑     [Request] [...]  │   │
│  └──────────────────────────────────────────────┘   │
│                                                     │
│  ┌─── STAT (col-3) ──┐ ┌── STAT ──┐ ┌── STAT ──┐ ┌── STAT ──┐
│  │ 💰 Income          │ │ 💸 Expense│ │ 🏃 Workout│ │ 📏 Distance│
│  │ Rp 12.378.200      │ │ Rp 5.788  │ │ 12 sesi   │ │ 87.4 km  │
│  │ +45% ↑             │ │ -12.6% ↓  │ │ +3 ↑      │ │ +22% ↑   │
│  └────────────────────┘ └───────────┘ └───────────┘ └──────────┘
│                                                     │
│  ┌─── CASH FLOW CHART (col-7) ───┐ ┌─── RIGHT PANEL (col-5) ──┐
│  │  [Weekly] [Daily] [Manage]    │ │  My Accounts             │
│  │                               │ │  ┌──────────────────────┐ │
│  │  [Bar chart cash flow]        │ │  │ BCA Savings  Rp 24M  │ │
│  │                               │ │  │ OVO          Rp 2.1M │ │
│  └───────────────────────────────┘ │  │ Cash         Rp 500K │ │
│                                    │  └──────────────────────┘ │
│                                    └──────────────────────────┘
│                                                     │
│  ┌─── RECENT TRANSACTIONS (col-7) ──┐ ┌─ WORKOUT SUMMARY (col-5) ┐
│  │ [Filter] [Sort] [...]            │ │  This Week               │
│  │ TYPE  AMOUNT   STATUS   METHOD   │ │  [Activity heatmap]      │
│  │ ────────────────────────────────  │ │                          │
│  │ + Gaji  Rp5M  ● Success  BCA    │ │  Last: Lari 5K — 28 min  │
│  │ ↑ BPJS  -Rp50K ○ Pending Gopay  │ │  [Run] [Ride] [Swim]     │
│  └──────────────────────────────────┘ └──────────────────────────┘
│                                                     │
│  ┌─── BUDGETS OVERVIEW (col-6) ────┐ ┌─ SAVING GOALS (col-6) ──┐
│  │  Makan      ████████░░ 80%      │ │  ✈ Liburan Bali    65%  │
│  │  Transport  ████░░░░░░ 40%      │ │  📱 iPhone 16      30%  │
│  │  Olahraga   ██████░░░░ 60%      │ │  🏠 DP Rumah        8%  │
│  └──────────────────────────────────┘ └────────────────────────┘
└─────────────────────────────────────────────────────┘
```

### 6.2 Transactions Page Layout

```
┌─────────────────────────────────────────────────────┐
│ TOPBAR                                              │
├─────────────────────────────────────────────────────┤
│ Page Header                                         │
│ Transactions          [+ Add Transaction]           │
│                                                     │
│ ┌─ FILTER BAR ──────────────────────────────────┐   │
│ │ [All] [Income] [Expense] [Transfer]           │   │
│ │ [Account ▾] [Category ▾] [Date Range ▾]       │   │
│ │                              [Sort ▾] [Export]│   │
│ └───────────────────────────────────────────────┘   │
│                                                     │
│ ┌─ SUMMARY STRIP ───────────────────────────────┐   │
│ │  Pemasukan: Rp 12.3M  ·  Pengeluaran: Rp 5.7M │   │
│ │  Net: +Rp 6.6M  (31 transaksi ditampilkan)    │   │
│ └───────────────────────────────────────────────┘   │
│                                                     │
│ ┌─ TRANSACTION TABLE ───────────────────────────┐   │
│ │ TYPE  DATE    DESCRIPTION   ACCOUNT  AMOUNT  STATUS │
│ │ ────────────────────────────────────────────  │   │
│ │ [Grouped by date, dengan divider]             │   │
│ │                                               │   │
│ │ Hari Ini                                      │   │
│ │ ↑  Jun 21  Gaji Bulanan    BCA     +Rp5M  ✓  │   │
│ │ ↓  Jun 21  Tokopedia       GoPay  -Rp125K ✓  │   │
│ │                                               │   │
│ │ Kemarin                                       │   │
│ │ ↓  Jun 20  PLN Token       Cash   -Rp50K  ✓  │   │
│ └───────────────────────────────────────────────┘   │
│                                                     │
│ [Pagination]                                        │
└─────────────────────────────────────────────────────┘
```

### 6.3 Workout / Activities Page

```
┌─────────────────────────────────────────────────────┐
│ Activities                    [+ Add] [↻ Sync Strava]│
│                                                     │
│ ┌─ PERSONAL RECORDS (col-12) ───────────────────┐   │
│ │  [🏃 Run: 42.2km]  [🚴 Ride: 6h12m]  [🏊 Swim: 3.2km] │
│ └───────────────────────────────────────────────┘   │
│                                                     │
│ ┌─ ACTIVITY CALENDAR HEATMAP (col-8) ──┐ ┌ STATS ─┐ │
│ │  Jan ─── Des                         │ │Minggu  │ │
│ │  [GitHub-style heatmap warna hijau]  │ │ini     │ │
│ │                                      │ │4 sesi  │ │
│ └──────────────────────────────────────┘ │28.4 km │ │
│                                          └────────┘ │
│                                                     │
│ ┌─ ACTIVITY LIST ────────────────────────────────┐  │
│ │ [All] [Run] [Ride] [Swim] [Walk] [Other]       │  │
│ │                                                │  │
│ │ ┌────────────────────────────────────────────┐ │  │
│ │ │ 🏃 Morning Run        Jun 21, 06:30         │ │  │
│ │ │    5.2 km · 28:14 · 5:26/km · 156 bpm     │ │  │
│ │ │    ↑ Personal Record — pace!               │ │  │
│ │ └────────────────────────────────────────────┘ │  │
│ └────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────┘
```

### 6.4 Strava Connect Page

```
┌─────────────────────────────────────────────────────┐
│ Strava Integration                                  │
│                                                     │
│  ┌─ STATUS CARD ─────────────────────────────────┐  │
│  │  [Strava Logo]                                │  │
│  │  Status: ● Connected / ○ Not Connected        │  │
│  │  Athlete: Nama Lengkap                        │  │
│  │  Last sync: 21 Jun 2026, 06:45               │  │
│  │                                               │  │
│  │  [↻ Sync Now]  [Disconnect]                   │  │
│  └───────────────────────────────────────────────┘  │
│                                                     │
│  ┌─ SYNC SETTINGS ───────────────────────────────┐  │
│  │  Auto-sync       [Toggle ON]                  │  │
│  │  Activity types  [✓ Run ✓ Ride ✓ Swim ...]   │  │
│  │  Sync since      [Date picker]                │  │
│  └───────────────────────────────────────────────┘  │
│                                                     │
│  ┌─ RECENTLY SYNCED ─────────────────────────────┐  │
│  │  [List 5 aktivitas terbaru dari Strava]       │  │
│  └───────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────┘
```

---

## 7. Animasi & Micro-interactions

### 7.1 Page Transitions

```css
/* Angular route animation (definisikan di app.component.ts) */
/* Gunakan @angular/animations dengan fade + slide up */

@keyframes ft-page-enter {
  from {
    opacity: 0;
    transform: translateY(8px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Apply ke router-outlet wrapper */
.page-container {
  animation: ft-page-enter 0.2s cubic-bezier(0.4, 0, 0.2, 1) forwards;
}
```

### 7.2 Number Counter (hero balance)

```css
/* CSS counter animation — angka naik saat load */
@keyframes ft-count-up {
  from { opacity: 0; transform: translateY(12px); }
  to   { opacity: 1; transform: translateY(0); }
}

.balance-value {
  animation: ft-count-up 0.4s cubic-bezier(0.4, 0, 0.2, 1) 0.1s both;
}
```

```typescript
// TypeScript helper untuk animasi angka
animateValue(element: HTMLElement, start: number, end: number, duration: number) {
  const range = end - start;
  const startTime = performance.now();
  const update = (currentTime: number) => {
    const elapsed = currentTime - startTime;
    const progress = Math.min(elapsed / duration, 1);
    const eased = 1 - Math.pow(1 - progress, 3); // ease-out-cubic
    element.textContent = this.formatCurrency(Math.floor(start + range * eased));
    if (progress < 1) requestAnimationFrame(update);
  };
  requestAnimationFrame(update);
}
```

### 7.3 Progress Bar (Budget)

```css
/* Animate budget progress bar saat masuk viewport */
.budget-progress-fill {
  width: 0%;
  animation: ft-budget-fill 0.8s cubic-bezier(0.34, 1.56, 0.64, 1) 0.2s forwards;
}

@keyframes ft-budget-fill {
  to { width: var(--budget-percentage); }
}
/* Set --budget-percentage via [style] binding di Angular */
```

### 7.4 Card Hover

```css
/* Smooth lift effect */
.card {
  transition:
    transform 200ms cubic-bezier(0.4, 0, 0.2, 1),
    box-shadow 200ms cubic-bezier(0.4, 0, 0.2, 1);
  will-change: transform;
}

.card:hover {
  transform: translateY(-2px);
  box-shadow: var(--shadow-lg);
}
```

### 7.5 Skeleton Loading

```css
/* Shimmer skeleton untuk saat data belum load */
.skeleton {
  background: linear-gradient(
    90deg,
    var(--bg-muted) 25%,
    var(--bg-canvas) 37%,
    var(--bg-muted) 63%
  );
  background-size: 400% 100%;
  animation: ft-shimmer 1.4s ease infinite;
  border-radius: var(--radius-md);
}

@keyframes ft-shimmer {
  0%   { background-position: 100% 50%; }
  100% { background-position: 0% 50%; }
}

/* Usage */
.skeleton-text { height: 16px; width: 60%; }
.skeleton-value { height: 32px; width: 40%; }
.skeleton-card { height: 120px; width: 100%; border-radius: var(--radius-xl); }
```

### 7.6 Toast Notification

```css
/* Toast container */
.toast-container {
  position: fixed;
  bottom: 24px;
  right: 24px;
  z-index: var(--z-toast);
  display: flex;
  flex-direction: column;
  gap: var(--space-3);
}

.toast {
  display: flex;
  align-items: flex-start;
  gap: var(--space-3);
  background: var(--text-primary);
  color: var(--text-inverse);
  padding: var(--space-4) var(--space-5);
  border-radius: var(--radius-xl);
  box-shadow: var(--shadow-xl);
  min-width: 300px;
  max-width: 420px;
  animation: ft-toast-in 0.3s var(--transition-spring);
}

@keyframes ft-toast-in {
  from { opacity: 0; transform: translateX(20px) scale(0.95); }
  to   { opacity: 1; transform: translateX(0) scale(1); }
}

.toast-success { border-left: 3px solid var(--income-500); }
.toast-error   { border-left: 3px solid var(--expense-500); }
.toast-info    { border-left: 3px solid var(--workout-accent); }
```

---

## 8. Tailwind Config Lengkap

```javascript
// tailwind.config.js
const defaultTheme = require('tailwindcss/defaultTheme');

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {

      // ── COLORS ──────────────────────────
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
        income: {
          50:  '#ecfdf5',
          100: '#d1fae5',
          500: '#10b981',
          600: '#059669',
          700: '#047857',
        },
        expense: {
          50:  '#fef2f2',
          100: '#fee2e2',
          500: '#ef4444',
          600: '#dc2626',
          700: '#b91c1c',
        },
        workout: {
          50:  '#f0f9ff',
          100: '#e0f2fe',
          500: '#0ea5e9',
          600: '#0284c7',
        },
        strava: '#FC4C02',
        canvas:  '#f6f8f7',
        surface: '#ffffff',
        sidebar: '#0f2219',
      },

      // ── FONTS ────────────────────────────
      fontFamily: {
        display: ['"Instrument Serif"', 'Georgia', 'serif'],
        sans: ['"DM Sans"', ...defaultTheme.fontFamily.sans],
        mono: ['"JetBrains Mono"', 'monospace'],
      },

      // ── FONT SIZE ────────────────────────
      fontSize: {
        '2xs': ['10px', { lineHeight: '14px', letterSpacing: '0.04em' }],
        'xs':  ['11px', { lineHeight: '16px' }],
        'sm':  ['12px', { lineHeight: '18px' }],
        'base':['14px', { lineHeight: '22px' }],
        'md':  ['16px', { lineHeight: '26px' }],
        'lg':  ['18px', { lineHeight: '28px' }],
        'xl':  ['20px', { lineHeight: '30px' }],
        '2xl': ['24px', { lineHeight: '32px', letterSpacing: '-0.008em' }],
        '3xl': ['28px', { lineHeight: '36px', letterSpacing: '-0.01em' }],
        '4xl': ['36px', { lineHeight: '44px', letterSpacing: '-0.015em' }],
        '5xl': ['44px', { lineHeight: '52px', letterSpacing: '-0.02em' }],
        '6xl': ['56px', { lineHeight: '64px', letterSpacing: '-0.025em' }],
      },

      // ── BORDER RADIUS ────────────────────
      borderRadius: {
        'xs': '4px',
        'sm': '6px',
        DEFAULT: '8px',
        'md': '8px',
        'lg': '12px',
        'xl': '16px',
        '2xl': '20px',
        '3xl': '24px',
      },

      // ── BOX SHADOW ───────────────────────
      boxShadow: {
        'xs':    '0 1px 2px rgba(15, 34, 25, 0.05)',
        'sm':    '0 1px 4px rgba(15, 34, 25, 0.07), 0 1px 2px rgba(15, 34, 25, 0.05)',
        DEFAULT: '0 2px 8px rgba(15, 34, 25, 0.06), 0 1px 3px rgba(15, 34, 25, 0.04)',
        'md':    '0 4px 12px rgba(15, 34, 25, 0.08), 0 2px 6px rgba(15, 34, 25, 0.05)',
        'lg':    '0 8px 24px rgba(15, 34, 25, 0.10), 0 4px 12px rgba(15, 34, 25, 0.06)',
        'xl':    '0 16px 40px rgba(15, 34, 25, 0.12), 0 8px 20px rgba(15, 34, 25, 0.07)',
        'brand': '0 4px 16px rgba(40, 90, 72, 0.22)',
        'brand-lg': '0 8px 24px rgba(40, 90, 72, 0.28)',
        'strava': '0 4px 14px rgba(252, 76, 2, 0.25)',
      },

      // ── SPACING ──────────────────────────
      spacing: {
        '4.5': '18px',
        '13':  '52px',
        '15':  '60px',
        '18':  '72px',
        '22':  '88px',
      },

      // ── BACKGROUND IMAGE ─────────────────
      backgroundImage: {
        'gradient-hero':    'linear-gradient(135deg, #285A48 0%, #1a3d31 50%, #0f2820 100%)',
        'gradient-sidebar': 'linear-gradient(180deg, #0f2219 0%, #0a1a14 100%)',
        'gradient-card':    'linear-gradient(135deg, #285A48 0%, #3d7a62 100%)',
        'gradient-income':  'linear-gradient(135deg, #059669 0%, #10b981 100%)',
        'gradient-expense': 'linear-gradient(135deg, #b91c1c 0%, #ef4444 100%)',
        'gradient-workout': 'linear-gradient(135deg, #0284c7 0%, #0ea5e9 100%)',
        'gradient-strava':  'linear-gradient(135deg, #FC4C02 0%, #ff6d33 100%)',
      },

      // ── TRANSITION ───────────────────────
      transitionTimingFunction: {
        'spring': 'cubic-bezier(0.34, 1.56, 0.64, 1)',
        'smooth': 'cubic-bezier(0.4, 0, 0.2, 1)',
      },
      transitionDuration: {
        '80': '80ms',
        '400': '400ms',
        '600': '600ms',
      },

      // ── ANIMATION ────────────────────────
      animation: {
        'shimmer':    'shimmer 1.4s ease infinite',
        'count-up':   'count-up 0.4s cubic-bezier(0.4, 0, 0.2, 1) 0.1s both',
        'fade-up':    'fade-up 0.2s cubic-bezier(0.4, 0, 0.2, 1) both',
        'toast-in':   'toast-in 0.3s cubic-bezier(0.34, 1.56, 0.64, 1)',
        'bounce-in':  'bounce-in 0.5s cubic-bezier(0.34, 1.56, 0.64, 1)',
      },
      keyframes: {
        shimmer: {
          '0%':   { backgroundPosition: '100% 50%' },
          '100%': { backgroundPosition: '0% 50%' },
        },
        'count-up': {
          'from': { opacity: '0', transform: 'translateY(12px)' },
          'to':   { opacity: '1', transform: 'translateY(0)' },
        },
        'fade-up': {
          'from': { opacity: '0', transform: 'translateY(8px)' },
          'to':   { opacity: '1', transform: 'translateY(0)' },
        },
        'toast-in': {
          'from': { opacity: '0', transform: 'translateX(20px) scale(0.95)' },
          'to':   { opacity: '1', transform: 'translateX(0) scale(1)' },
        },
        'bounce-in': {
          'from': { opacity: '0', transform: 'scale(0.9)' },
          'to':   { opacity: '1', transform: 'scale(1)' },
        },
      },

      // ── SCREENS ──────────────────────────
      screens: {
        'xs':  '480px',
        'sm':  '640px',
        'md':  '768px',
        'lg':  '1024px',
        'xl':  '1280px',
        '2xl': '1440px',
        '3xl': '1600px',
      },
    },
  },
  plugins: [],
};
```

---

## 9. NG-Zorro Theme Override

```less
// src/styles/ng-zorro-theme.less
// Paste BEFORE importing ng-zorro-antd.less

// ── CORE ────────────────────────────────
@primary-color:      #285A48;
@primary-color-hover: #2d7a64;
@link-color:         #285A48;
@success-color:      #059669;
@info-color:         #0ea5e9;
@warning-color:      #f59e0b;
@error-color:        #dc2626;

// ── BORDER ──────────────────────────────
@border-color-base:  rgba(40, 90, 72, 0.15);
@border-color-split: rgba(40, 90, 72, 0.08);
@border-radius-base: 8px;
@border-width-base:  1px;

// ── TYPOGRAPHY ──────────────────────────
@font-family:        'DM Sans', -apple-system, BlinkMacSystemFont, sans-serif;
@font-size-base:     14px;
@font-size-sm:       12px;
@font-size-lg:       16px;
@heading-color:      #0f2219;
@text-color:         #3d5a50;
@text-color-secondary: #7a9d91;
@disabled-color:     rgba(0, 0, 0, 0.25);

// ── LAYOUT ──────────────────────────────
@layout-body-background:    #f6f8f7;
@layout-header-background:  #ffffff;
@layout-sider-background:   #0f2219;
@layout-header-height:      64px;
@layout-header-padding:     0 24px;

// ── SIDEBAR / MENU ───────────────────────
@menu-bg:                   transparent;
@menu-dark-bg:              #0f2219;
@menu-dark-submenu-bg:      #1a3529;
@menu-item-active-bg:       #285A48;
@menu-dark-item-active-bg:  #285A48;
@menu-dark-color:           #7fa898;
@menu-dark-selected-item-text-color: #ffffff;
@menu-dark-selected-item-icon-color: #ffffff;
@menu-dark-item-hover-bg:   #1a3529;
@menu-icon-size:             16px;
@menu-item-height:           40px;
@menu-item-boundary-margin:  4px;
@menu-inline-toplevel-item-height: 40px;

// ── BUTTON ──────────────────────────────
@btn-font-weight:     500;
@btn-border-radius-base: 8px;
@btn-height-base:     38px;
@btn-height-sm:       28px;
@btn-height-lg:       46px;
@btn-font-size-sm:    12px;
@btn-font-size-lg:    15px;
@btn-padding-horizontal-base: 18px;
@btn-primary-color:    #ffffff;
@btn-primary-bg:       #285A48;
@btn-default-border:   rgba(40, 90, 72, 0.2);
@btn-shadow:           0 4px 16px rgba(40, 90, 72, 0.22);
@btn-primary-shadow:   0 4px 16px rgba(40, 90, 72, 0.22);

// ── CARD ────────────────────────────────
@card-head-color:         #0f2219;
@card-head-font-size:     15px;
@card-head-font-size-sm:  13px;
@card-head-padding:       16px 24px;
@card-inner-head-padding: 12px 24px;
@card-padding-base:       24px;
@card-padding-base-sm:    16px;
@card-radius:             16px;
@card-shadow:             0 2px 8px rgba(15, 34, 25, 0.06), 0 1px 3px rgba(15, 34, 25, 0.04);
@card-background:         #ffffff;

// ── TABLE ────────────────────────────────
@table-header-bg:           #f6f8f7;
@table-header-color:        #7a9d91;
@table-header-sort-bg:      #f0f4f2;
@table-body-sort-bg:        #fafbfa;
@table-row-hover-bg:        #f6f8f7;
@table-selected-row-bg:     #f0f7f4;
@table-border-color:        rgba(40, 90, 72, 0.08);
@table-border-radius-base:  16px;
@table-footer-bg:           #f6f8f7;
@table-header-cell-split-color: rgba(40, 90, 72, 0.08);

// ── INPUT ────────────────────────────────
@input-height-base:    38px;
@input-height-lg:      46px;
@input-height-sm:      28px;
@input-border-color:   rgba(40, 90, 72, 0.2);
@input-hover-border-color: #285A48;
@input-bg:             #ffffff;
@input-addon-bg:       #f6f8f7;
@input-color:          #0f2219;
@input-placeholder-color: #7a9d91;
@input-focus-border-color: #285A48;
@input-prefix-padding: 0 12px;

// ── MODAL ────────────────────────────────
@modal-header-bg:          #ffffff;
@modal-header-border-color: rgba(40, 90, 72, 0.08);
@modal-content-bg:         #ffffff;
@modal-footer-bg:          #f6f8f7;
@modal-close-color:        #7a9d91;
@modal-mask-bg:            rgba(15, 34, 25, 0.55);
@modal-header-padding:     20px 24px;
@modal-body-padding:       24px;
@modal-footer-padding-vertical: 16px;
@modal-border-radius:      20px;

// ── SELECT ───────────────────────────────
@select-border-color:      rgba(40, 90, 72, 0.2);
@select-item-selected-bg:  #f0f7f4;
@select-item-selected-font-weight: 500;
@select-item-active-bg:    #f6f8f7;
@select-background:        #ffffff;

// ── NOTIFICATION / MESSAGE ────────────────
@notification-border-radius: 12px;
@notification-width:         380px;

// ── DATEPICKER ──────────────────────────
@calendar-bg:          #ffffff;
@date-picker-active-bg: #f0f7f4;
@picker-active-color:  #285A48;
@picker-border-color:  rgba(40, 90, 72, 0.2);

// ── TAG ─────────────────────────────────
@tag-default-bg:        #f0f7f4;
@tag-default-color:     #3d5a50;
@tag-font-size:         11px;
@tag-border-radius:     6px;

// ── PROGRESS ─────────────────────────────
@process-tail-color:   #285A48;
@progress-default-color: #285A48;
@progress-remaining-color: rgba(40, 90, 72, 0.1);
```

---

## 10. Panduan Gemini AI Prompt

Gunakan prompt template berikut saat meminta Gemini AI untuk coding di VS Code:

### Template Prompt Umum

```
Kamu adalah Angular 18 developer expert. Saya membangun aplikasi FiTrack
(Personal Finance + Workout Tracker) dengan:
- Angular 18 (standalone components)
- Tailwind CSS (config terlampir)
- NG-Zorro (tema override terlampir)
- TypeScript strict mode

DESIGN TOKENS (sudah didefinisikan di tokens.css):
- Primary: #285A48
- Background: #f6f8f7 (canvas), #ffffff (surface)
- Sidebar: #0f2219
- Font: 'DM Sans' (body), 'Instrument Serif' (display/angka besar)
- Border radius card: 16px (radius-xl)
- Shadow card: 0 2px 8px rgba(15, 34, 25, 0.06)

ATURAN CODING:
- Gunakan CSS custom properties (var(--nama-token)) bukan hardcode hex
- Tailwind class untuk layout/spacing, custom CSS untuk komponen kompleks
- NG-Zorro untuk: Table, Modal, Form, DatePicker, Select, Notification
- Semua komponen harus standalone dan menggunakan inject() bukan constructor DI
- Gunakan signal() untuk state management lokal
- Lazy load setiap feature module

[Tambahkan permintaan spesifik di sini]
```

### Prompt Spesifik per Komponen

**Dashboard:**
```
Buatkan dashboard.component.ts dan dashboard.component.html untuk FiTrack.
Gunakan layout grid 12 kolom. Komponen yang diperlukan:
1. Hero card (full width) dengan total saldo menggunakan font Instrument Serif 44px
2. 4 stat card (masing-masing 3 kolom): Income, Expense, Total Workout, Distance
3. Bar chart Cash Flow (col-7) menggunakan ng2-charts
4. Panel akun (col-5) dengan list rekening

Ikuti design tokens: card border-radius 16px, shadow var(--shadow-card),
warna income: #059669, expense: #dc2626, brand: #285A48.
```

**Sidebar:**
```
Buatkan sidebar.component.ts dengan:
- Background gradient dari #0f2219 ke #0a1a14
- Logo FiTrack di atas
- Menu grup: GENERAL, FINANCE, WORKOUT, INSIGHTS
- Active item: background #285A48, left indicator 3px hijau
- Bottom: user card dengan avatar + nama + email
- Badge "Connected" (hijau) atau "Connect" (oranye Strava) di menu Strava Sync
- Width 240px, scrollable, no scrollbar visible
```

**Transaction Table:**
```
Buatkan transaction-list.component menggunakan nz-table dengan:
- Filter bar: chip [All/Income/Expense/Transfer], dropdown Account & Category
- Grouping by date dengan sticky date header
- Kolom: Type icon, Date, Description, Account, Amount (font mono), Status badge
- Amount: hijau (#059669) untuk income, merah (#dc2626) untuk expense
- Hover row: background #f6f8f7
- Empty state dengan ilustrasi dan tombol Add Transaction
```

### Checklist Sebelum Submit ke Gemini

- [ ] Sebutkan Angular 18 standalone component
- [ ] Cantumkan token warna yang relevan
- [ ] Sebutkan NG-Zorro component yang dipakai
- [ ] Sertakan ukuran/dimensi spesifik (border-radius, shadow, font-size)
- [ ] Minta TypeScript typing yang ketat
- [ ] Minta animasi jika ada (transisi, skeleton, counter)

---

## Appendix: Quick Reference Cheatsheet

### Warna Cepat

```
Hijau brand:    #285A48  var(--brand-600)
Income:         #059669  var(--income-600)
Expense:        #dc2626  var(--expense-600)
Workout/Biru:   #0ea5e9  var(--workout-accent)
Strava:         #FC4C02  var(--strava-orange)
BG halaman:     #f6f8f7  var(--bg-canvas)
BG card:        #ffffff  var(--bg-surface)
Sidebar:        #0f2219  var(--sidebar-bg)
Teks utama:     #0f2219  var(--text-primary)
Teks sekunder:  #3d5a50  var(--text-secondary)
Teks redup:     #7a9d91  var(--text-muted)
```

### Spacing Cepat

```
4px   var(--space-1)   Tabler icon gap, badge padding vertikal
8px   var(--space-2)   Gap antara icon & label
12px  var(--space-3)   Nav item margin horizontal
16px  var(--space-4)   Grid gap, cell padding table
20px  var(--space-5)   Card padding (kecil)
24px  var(--space-6)   Card padding default, content padding
32px  var(--space-8)   Hero card padding, section gap
```

### Border Radius Cepat

```
4px   var(--radius-xs)   Badge, indicator dot
6px   var(--radius-sm)   Button kecil, tag
8px   var(--radius-md)   Button default, input, tooltip
12px  var(--radius-lg)   Dropdown, sub-card, nav item hover
16px  var(--radius-xl)   Card utama ← PALING SERING DIPAKAI
20px  var(--radius-2xl)  Hero card, modal
```

### Shadow Cepat

```
var(--shadow-xs)    Elemen inline, divider
var(--shadow-sm)    Input focus, hover state kecil
var(--shadow-card)  Card default ← PALING SERING DIPAKAI
var(--shadow-md)    Card hover, dropdown
var(--shadow-lg)    Modal, overlay panel
var(--shadow-brand) Tombol primary, nav active item
```

---

> **Versi:** 1.0 · FiTrack Frontend Styling Guide
> **Diperbarui:** Mei 2026
> **Untuk digunakan bersama:** Gemini AI di VS Code, Angular 18, Tailwind CSS, NG-Zorro
