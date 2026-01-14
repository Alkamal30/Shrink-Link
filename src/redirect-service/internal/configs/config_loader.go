package configs

import (
	"log/slog"
	"os"
	"strings"

	"github.com/spf13/viper"
)

func LoadConfig() (*AppConfig, error) {
	env := getEnv()
	slog.Info("Loading configuration", "env", env)

	viper.SetConfigName("config")
	viper.SetConfigType("yaml")
	viper.AddConfigPath("./configs")
	viper.AddConfigPath("./internal/configs")
	viper.AutomaticEnv()
	viper.SetEnvKeyReplacer(strings.NewReplacer(".", "_"))

	if err := viper.ReadInConfig(); err != nil {
		slog.Error("Failed to read base config", "err", err)
		return nil, err
	}

	viper.SetConfigName("config." + env)
	if err := viper.MergeInConfig(); err != nil {
		slog.Error("Failed to read env config", "err", err)
		return nil, err
	}

	var appConfig AppConfig
	if err := viper.Unmarshal(&appConfig); err != nil {
		slog.Error("Failed to unmarshal config", "err", err)
		return nil, err
	}

	slog.Info("Configuration loaded")

	return &appConfig, nil
}

func getEnv() string {
	env := os.Getenv("APP_ENV")
	if env == "" {
		env = "dev"
	}

	return strings.ToLower(env)
}
