-- PostgreSQL Schema

-- Users & Auth
CREATE TABLE users (
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username    VARCHAR(50) UNIQUE NOT NULL,
    email       VARCHAR(255) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    strava_athlete_id VARCHAR(50),
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE user_sessions (
    id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id           UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    refresh_token_hash TEXT NOT NULL,
    device_info       TEXT,
    ip_address        VARCHAR(45),
    created_at        TIMESTAMPTZ DEFAULT NOW(),
    expires_at        TIMESTAMPTZ NOT NULL,
    revoked_at        TIMESTAMPTZ
);

CREATE TABLE strava_tokens (
    id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id       UUID UNIQUE NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    access_token  TEXT NOT NULL,
    refresh_token TEXT NOT NULL,
    expires_at    TIMESTAMPTZ NOT NULL,
    athlete_id    VARCHAR(50) NOT NULL,
    updated_at    TIMESTAMPTZ DEFAULT NOW()
);

-- Finance
CREATE TABLE accounts (
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id     UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name        VARCHAR(100) NOT NULL,
    type        VARCHAR(30) NOT NULL, -- cash, bank, ewallet, investment
    balance     NUMERIC(15,2) DEFAULT 0,
    color       VARCHAR(7),
    icon        VARCHAR(50),
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE categories (
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id     UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name        VARCHAR(100) NOT NULL,
    type        VARCHAR(10) NOT NULL, -- income / expense
    icon        VARCHAR(50),
    color       VARCHAR(7),
    is_default  BOOLEAN DEFAULT FALSE,
    created_at  TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE transactions (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    account_id      UUID NOT NULL REFERENCES accounts(id),
    category_id     UUID NOT NULL REFERENCES categories(id),
    type            VARCHAR(10) NOT NULL, -- income / expense / transfer
    amount          NUMERIC(15,2) NOT NULL,
    description     VARCHAR(255) NOT NULL,
    notes           TEXT,
    date            DATE NOT NULL,
    created_at      TIMESTAMPTZ DEFAULT NOW()
);
CREATE INDEX idx_transactions_user_date ON transactions(user_id, date);

CREATE TABLE budgets (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    category_id     UUID NOT NULL REFERENCES categories(id),
    amount          NUMERIC(15,2) NOT NULL,
    month           SMALLINT NOT NULL, -- 1-12
    year            SMALLINT NOT NULL,
    created_at      TIMESTAMPTZ DEFAULT NOW(),
    UNIQUE(user_id, category_id, month, year)
);

CREATE TABLE saving_goals (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name            VARCHAR(100) NOT NULL,
    target_amount   NUMERIC(15,2) NOT NULL,
    current_amount  NUMERIC(15,2) DEFAULT 0,
    target_date     DATE,
    is_completed    BOOLEAN DEFAULT FALSE,
    created_at      TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE recurring_bills (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    category_id     UUID REFERENCES categories(id),
    name            VARCHAR(100) NOT NULL,
    amount          NUMERIC(15,2) NOT NULL,
    due_day         SMALLINT NOT NULL, -- 1-31
    is_active       BOOLEAN DEFAULT TRUE,
    last_reminded   TIMESTAMPTZ,
    created_at      TIMESTAMPTZ DEFAULT NOW()
);

-- Workout
CREATE TABLE activities (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id             UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    strava_activity_id  BIGINT UNIQUE,
    type                VARCHAR(50) NOT NULL, -- run, ride, swim, walk, etc.
    name                VARCHAR(255) NOT NULL,
    distance_meters     FLOAT,
    duration_seconds    INT NOT NULL,
    elevation_gain_m    FLOAT,
    calories_burned     INT,
    average_pace        FLOAT, -- sec/km
    average_heart_rate  FLOAT,
    start_date          TIMESTAMPTZ NOT NULL,
    is_from_strava      BOOLEAN DEFAULT FALSE,
    created_at          TIMESTAMPTZ DEFAULT NOW()
);
CREATE INDEX idx_activities_user_date ON activities(user_id, start_date);