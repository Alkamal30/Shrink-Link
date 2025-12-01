package analytics

import (
	"net"
	"time"
)

type RedirectAnalyticsData struct {
	ShortCode string
	Timestamp time.Time
	Ip        net.IP
	UserAgent string
	Referer   string
	Language  string
}
