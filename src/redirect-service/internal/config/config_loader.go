package config

import (
	"os"
	"strings"

	"github.com/spf13/viper"
)

func LoadConfig() (*AppConfig, error) {
	env := getEnv()

	viper.SetConfigName("config")
	viper.SetConfigType("yaml")
	viper.AddConfigPath("./internal/config")

	if err := viper.ReadInConfig(); err != nil {
		return nil, err
	}

	viper.SetConfigName("config." + env)
	if err := viper.MergeInConfig(); err != nil {
		return nil, err
	}

	var appConfig AppConfig
	if err := viper.Unmarshal(&appConfig); err != nil {
		return nil, err
	}

	return &appConfig, nil
}

func getEnv() string {
	env := os.Getenv("APP_ENV")
	if env == "" {
		env = "dev"
	}

	return strings.ToLower(env)
}
