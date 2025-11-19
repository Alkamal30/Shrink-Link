package grpcclient

import (
	"crypto/tls"
	"crypto/x509"
	"fmt"
	"os"
	"redirect-service/internal/configs"

	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials"
	"google.golang.org/grpc/credentials/insecure"
)

func NewClientConnection(cfg configs.GrpcConfig) (*grpc.ClientConn, error) {
	if !cfg.UseTls {
		return grpc.NewClient(
			cfg.Url,
			grpc.WithTransportCredentials(insecure.NewCredentials()),
		)
	}

	tlsConfig := &tls.Config{}

	if cfg.CertificatePath != "" {
		certPool := x509.NewCertPool()

		caCert, err := os.ReadFile(cfg.CertificatePath)
		if err != nil {
			return nil, fmt.Errorf("cannot read CA certificate from %s: %w", cfg.CertificatePath, err)
		}

		if ok := certPool.AppendCertsFromPEM(caCert); !ok {
			return nil, fmt.Errorf("failed to append CA certificate")
		}

		tlsConfig.RootCAs = certPool
	}

	creds := credentials.NewTLS(tlsConfig)

	return grpc.NewClient(
		cfg.Url,
		grpc.WithTransportCredentials(creds),
	)
}
