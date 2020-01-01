CREATE TABLE IF NOT EXISTS "_migrations" (
  "id" INTEGER PRIMARY KEY AUTOINCREMENT,
  "batch_id" INTEGER NOT NULL,
  "file_name" TEXT NOT NULL,
  "created_at" TEXT NOT NULL
);
