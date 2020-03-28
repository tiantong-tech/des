create table if not exists goods (
  id serial not null primary key,
  warehouse_id int not null,
  category_ids int[] not null,
  item_ids int[] not null,
  stock_ids int[] not null,
  number varchar(255),
  name varchar(255) not null,
  comment varchar(255) not null,
  is_enabled boolean not null,
  created_at timestamp(0) not null,
  unique(warehouse_id, number)
);
