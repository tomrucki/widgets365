# Sensors

## for future improvement

- requirement for the library api is satisfied, but for the future a stream input or file name should be considered to avoid memory issues.
- currently log time is not utilized, it could be omitted and save space. Alternative solution could be to log only a test start time and logging interval.
- depending on the needs - output could be strongly typed and leave the serialization to the consumer
- for simpler and more robust processing it would be beneficial to have a log file per sensor instead of one shared one
- reference values could be clearly tagged, so we don't rely on specific order to know what value is reference temperature, etc.
- limits for policies are static, library could allow to specify them as a parameter
- add documentation

## other comments

- simplified value requirement - all logged values are treated as decimal
- assumption that no logged values for given sensor is an error
