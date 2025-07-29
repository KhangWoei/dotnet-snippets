create table trees (
    id SERIAL PRIMARY KEY, 
    name varchar(255) not null, 
    base_url text, 
    created_at timestamptz default now(), 
    constraint unique_trees_name UNIQUE(name)
);

create table nodes (
    id bigserial primary key, 
    tree_id integer not null references trees(id) on delete cascade, 
    parent_id bigint references nodes(id) on delete cascade, 
    full_path text not null,
    is_terminal boolean default false, 
    created_at timestamptz default now()
);
