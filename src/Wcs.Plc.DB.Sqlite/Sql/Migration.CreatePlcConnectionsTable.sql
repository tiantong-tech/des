CREATE TABLE IF NOT EXISTS "plc_connections" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "model" TEXT NOT NULL,
  "name" TEXT NOT NULL UNIQUE,
  "host" TEXT,
  "port" TEXT,
  "created_at" TEXT NOT NULL
);
