package configs

type AppConfig struct {
	Url   string
	Grpc  GrpcConfig
	Redis RedisConfig
	Kafka KafkaConfig
}

type GrpcConfig struct {
	Url             string
	UseTls          bool
	CertificatePath string
}

type RedisConfig struct {
	Address  string
	Password string
}

type KafkaConfig struct {
	Brokers                []string
	RedirectAnalyticsTopic string
}
