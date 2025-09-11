# Managing consumer groups

Base command
```
kafka-consumer-groups.sh --bootstrap-server <server>
```

1. List groups
```
--list
```

2. View topic offsets in a group
```
--describe --group <group>
```

3. View active members
```
--members --group <group>
```

4. Deleting a group
```
--delete --group <group>
```

5. Reset offsets

```
<group_scope> <topic_scope> --reset-offsets  --to-earliest <running_mode>
```
<new_offset>:
    --to-datetime, 
    --by-duration, 
    --to-earliest, 
    --to-latest,
    --shift-by,
    --from-file,
    --to-current, 
    --to-offset.             

<group_scope>:
    --all-groups
    --group <group_name>

<topic_scope>:
    --all-topics   
    --topic <topic_name>
    --from-file <file_path>

<running_mode>
    --dry-run
    --execute
