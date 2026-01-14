package grpcclient

import (
	"crypto/tls"
	"crypto/x509"
	"fmt"
	"log/slog"
	"os"
	"redirect-service/internal/configs"

	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials"
	"google.golang.org/grpc/credentials/insecure"
)

func NewClientConnection(cfg configs.GrpcConfig) (*grpc.ClientConn, error) {
	slog.Info("Creating new gRPC client connection", "url", cfg.Url, "useTls", cfg.UseTls)

	if !cfg.UseTls {
		slog.Debug("Using insecure gRPC credentials")
		return grpc.NewClient(
			cfg.Url,
			grpc.WithTransportCredentials(insecure.NewCredentials()),
		)
	}

	slog.Debug("Using TLS gRPC credentials")
	tlsConfig := &tls.Config{}

	if cfg.CertificatePath != "" {
		slog.Info("Loading CA certificate for gRPC connection", "path", cfg.CertificatePath)
		certPool := x509.NewCertPool()

		caCert, err := os.ReadFile(cfg.CertificatePath)
		if err != nil {
			slog.Error("Failed to read CA certificate", "path", cfg.CertificatePath, "err", err)
			return nil, fmt.Errorf("cannot read CA certificate from %s: %w", cfg.CertificatePath, err)
		}

		if ok := certPool.AppendCertsFromPEM(caCert); !ok {
			slog.Error("Failed to append CA certificate to pool", "path", cfg.CertificatePath)
			return nil, fmt.Errorf("failed to append CA certificate")
		}

		tlsConfig.RootCAs = certPool
	}

	creds := credentials.NewTLS(tlsConfig)

	conn, err := grpc.NewClient(
		cfg.Url,
		grpc.WithTransportCredentials(creds),
	)
	if err != nil {
		slog.Error("Failed to create gRPC client connection", "url", cfg.Url, "err", err)
		return nil, err
	}

	slog.Info("Successfully created gRPC client connection", "url", cfg.Url)
	return conn, nil
}
