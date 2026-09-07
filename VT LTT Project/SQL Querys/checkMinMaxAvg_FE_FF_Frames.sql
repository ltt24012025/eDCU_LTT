/* Set the database/schema name and table name /Set the TEST_MODE search pattern */
SET @schema_name = 'edcu_48v_thermal_endurance_test_pv_24_08_2026_18_46_06';
SET @table_name  = 'fe_frame_log';
SET @test_mode_pattern = '%Mode 11%';

/* Set the number of initial columns to exclude */
SET @exclude_count = 5;

/* Set the required number of decimal places */
SET @decimal_places = 3;

/* Increase the maximum length for the dynamically generated query */
SET SESSION group_concat_max_len = 1000000;

/* Generate one result row per numeric column after the excluded columns */
SELECT GROUP_CONCAT(
    CONCAT(
        'SELECT ''', COLUMN_NAME, ''' AS column_name, ',
        'ROUND(MIN(`', COLUMN_NAME, '`), ', @decimal_places, ') AS min_value, ',
        'ROUND(MAX(`', COLUMN_NAME, '`), ', @decimal_places, ') AS max_value, ',
        'ROUND(AVG(`', COLUMN_NAME, '`), ', @decimal_places, ') AS avg_value, ',
        'ROUND(MAX(`', COLUMN_NAME, '`) - MIN(`', COLUMN_NAME, '`), ',
        @decimal_places, ') AS diff ',
        'FROM `', @schema_name, '`.`', @table_name, '` ',
        'WHERE `TEST_MODE` LIKE ',
        QUOTE(@test_mode_pattern)
    )
    ORDER BY ORDINAL_POSITION
    SEPARATOR ' UNION ALL '
) INTO @sql
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = @schema_name
  AND TABLE_NAME = @table_name
  AND ORDINAL_POSITION > @exclude_count
  AND DATA_TYPE IN (
      'tinyint',
      'smallint',
      'mediumint',
      'int',
      'integer',
      'bigint',
      'decimal',
      'numeric',
      'float',
      'double',
      'real'
  );

/* Execute the generated UNION ALL query */
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;