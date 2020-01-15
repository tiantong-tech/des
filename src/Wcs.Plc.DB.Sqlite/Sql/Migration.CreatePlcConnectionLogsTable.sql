CREATE TABLE IF NOT EXISTS "plc_connection_logs" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "plc_id" INTEGER NOT NULL,
  "operation" TEXT NOT NULL,
  "created_at" TEXT NOT NULL
);
