package analytics

import (
	"time"
)

type RedirectAnalyticsData struct {
	ShortCode string
	Timestamp time.Time
	Ip        string
	UserAgent string
	Referer   string
	Language  string
}
