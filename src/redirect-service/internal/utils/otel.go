package utils

import (
	"context"

	"go.opentelemetry.io/otel/attribute"
	"go.opentelemetry.io/otel/exporters/otlp/otlplog/otlploggrpc"
	"go.opentelemetry.io/otel/log/global"
	sdklog "go.opentelemetry.io/otel/sdk/log"
	"go.opentelemetry.io/otel/sdk/resource"
)

func InitOTelLogging(ctx context.Context) (func(context.Context) error, error) {
	exp, err := otlploggrpc.New(ctx)
	if err != nil {
		return nil, err
	}

	res, err := resource.Merge(
		resource.Default(),
		resource.NewWithAttributes("", attribute.String("service.name", "redirect-service")),
	)
	if err != nil {
		return nil, err
	}

	provider := sdklog.NewLoggerProvider(
		sdklog.WithResource(res),
		sdklog.WithProcessor(sdklog.NewBatchProcessor(exp)),
	)

	global.SetLoggerProvider(provider)
	return provider.Shutdown, nil
}
